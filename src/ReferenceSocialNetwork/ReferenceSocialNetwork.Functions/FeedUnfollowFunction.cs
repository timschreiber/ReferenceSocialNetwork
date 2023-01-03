using CheesyTot.AzureTables.SimpleIndex.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ReferenceSocialNetwork.Common.Data.Entities;

namespace ReferenceSocialNetwork.Functions
{
    internal class FeedUnfollowFunction
    {
        private readonly ILogger _logger;
        private readonly ISimpleIndexRepository<FeedItem> _feedItemRepository;

        public FeedUnfollowFunction(ILogger logger, ISimpleIndexRepository<FeedItem> feedItemRepository)
        {
            _logger = logger;
            _feedItemRepository = feedItemRepository;
        }

        [Function("FeedUnfollowFunction")]
        public async Task Run([QueueTrigger("feedunfollow", Connection = "StorageConnectionString")]string followId)
        {
            _logger.LogInformation($"{nameof(FeedUnfollowFunction)} started processing profileId: {followId}");

            var feedItems = await _feedItemRepository.GetAsync(followId);

            // Can optimize this with batch operations later...
            if((feedItems?.Any() ?? false) == true)
            {
                foreach(var feedItem in feedItems)
                {
                    await _feedItemRepository.DeleteAsync(feedItem);
                }
            }

            _logger.LogInformation($"{nameof(FeedUnfollowFunction)} finished processing postId: {followId}");
        }
    }
}
