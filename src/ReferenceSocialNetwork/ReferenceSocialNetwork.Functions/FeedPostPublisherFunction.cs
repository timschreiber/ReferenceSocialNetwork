using CheesyTot.AzureTables.SimpleIndex.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ReferenceSocialNetwork.Common.Data.Entities;

namespace ReferenceSocialNetwork.Functions
{
    public class FeedPostPublisherFunction
    {
        private readonly ILogger _logger;
        private readonly ISimpleIndexRepository<FeedItem> _feedItemRepository;
        private readonly ISimpleIndexRepository<Follow> _followRepository;
        private readonly ISimpleIndexRepository<Post> _postRepository;
        private readonly ISimpleIndexRepository<Profile> _profileRepository;

        public FeedPostPublisherFunction(ILogger logger,
            ISimpleIndexRepository<FeedItem> feedItemRepository,
            ISimpleIndexRepository<Follow> followRepository,
            ISimpleIndexRepository<Post> postRepository,
            ISimpleIndexRepository<Profile> profileRepository)
        {
            _logger = logger;
            _feedItemRepository = feedItemRepository;
            _followRepository = followRepository;
            _postRepository = postRepository;
            _profileRepository = profileRepository;
        }

        [Function("FeedPostPublisherFunction")]
        public async Task Run([QueueTrigger("feedpostpublisher", Connection = "StorageConnectionString")]string postId)
        {
            _logger.LogInformation($"{nameof(FeedPostPublisherFunction)} started processing postId: {postId}");
            var strProfileId = postId[16..];
            var strPostDate = postId[..16];

            var profile = await _profileRepository.GetSingleByIndexedPropertyAsync(nameof(Profile.ProfileId), strProfileId);
            if(profile == null)
            {
                _logger.LogInformation($"Invalid profile: {strProfileId}");
                return;
            }

            var post = await _postRepository.GetAsync(strProfileId, strPostDate);
            if(post == null)
            {
                _logger.LogInformation($"Invalid post: {strPostDate}");
                return;
            }

            var follows = await _followRepository.GetByIndexedPropertyAsync(nameof(Follow.FollowedProfileId), profile.ProfileId);
            if((follows?.Any() ?? false) == false)
            {
                _logger.LogInformation($"No followers for profile: {strProfileId}");
                return;
            }

            foreach(var follow in follows)
            {
                var feedItem = new FeedItem(follow.FollowerProfileId, postId);
                await _feedItemRepository.AddAsync(feedItem);
            }

            _logger.LogInformation($"{nameof(FeedPostPublisherFunction)} finished processing postId: {postId}");
        }
    }
}
