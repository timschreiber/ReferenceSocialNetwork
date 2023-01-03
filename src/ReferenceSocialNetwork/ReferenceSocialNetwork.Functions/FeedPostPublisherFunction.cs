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
            var objPostId = new PostId(postId);
            var profile = await _profileRepository.GetSingleByIndexedPropertyAsync(nameof(Profile.ProfileId), objPostId.StrProfileId);
            if(profile == null)
            {
                _logger.LogInformation($"Invalid profile: {objPostId.StrProfileId}");
                return;
            }

            var post = await _postRepository.GetAsync(objPostId.StrProfileId, objPostId.StrEncDateTime);
            if(post == null)
            {
                _logger.LogInformation($"Invalid post: {objPostId.StrEncDateTime}");
                return;
            }

            var follows = await _followRepository.GetByIndexedPropertyAsync(nameof(Follow.FollowedProfileId), profile.ProfileId);
            if((follows?.Any() ?? false) == false)
            {
                _logger.LogInformation($"No followers for profile: {objPostId.StrProfileId}");
                return;
            }

            foreach(var follow in follows)
            {
                var feedItem = new FeedItem(follow.FollowId, objPostId);
                await _feedItemRepository.AddAsync(feedItem);
            }

            _logger.LogInformation($"{nameof(FeedPostPublisherFunction)} finished processing postId: {postId}");
        }
    }
}
