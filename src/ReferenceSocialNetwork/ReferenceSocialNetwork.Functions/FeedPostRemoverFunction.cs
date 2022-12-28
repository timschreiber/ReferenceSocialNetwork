using CheesyTot.AzureTables.SimpleIndex.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ReferenceSocialNetwork.Common.Data.Entities;

namespace ReferenceSocialNetwork.Functions
{
    internal class FeedPostRemoverFunction
    {
        private readonly ILogger _logger;
        private readonly ISimpleIndexRepository<FeedItem> _feedItemRepository;

        public FeedPostRemoverFunction(ILogger logger,
            ISimpleIndexRepository<FeedItem> feedItemRepository)
        {
            _logger = logger;
            _feedItemRepository = feedItemRepository;
        }

        [Function("FeedPostRemoverFunction")]
        public async Task Run([QueueTrigger("feedpostremover", Connection = "StorageConnectionString")]string postId)
        {
            _logger.LogInformation($"{nameof(FeedPostRemoverFunction)} started processing postId: {postId}");

            var feedItems = await _feedItemRepository.GetByIndexedPropertyAsync(nameof(FeedItem.PostId), postId);
            if((feedItems?.Any() ?? false) == false)
            {
                _logger.LogInformation($"No feed items for postId: {postId}");
                return;
            }

            foreach(var feedItem in feedItems)
            {
                await _feedItemRepository.DeleteAsync(feedItem);
            }

            _logger.LogInformation($"{nameof(FeedPostRemoverFunction)} finished processing postId: {postId}");
        }
    }
}
