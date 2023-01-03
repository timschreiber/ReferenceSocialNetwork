using CheesyTot.AzureTables.SimpleIndex.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ReferenceSocialNetwork.Common.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceSocialNetwork.Functions
{
    internal class FeedFollowFunction
    {
        private readonly ILogger _logger;
        private readonly ISimpleIndexRepository<Post> _postRepository;
        private readonly ISimpleIndexRepository<FeedItem> _feedItemRepository;

        public FeedFollowFunction(ILogger logger,
            ISimpleIndexRepository<Post> postRepository,
            ISimpleIndexRepository<FeedItem> feedItemRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _feedItemRepository = feedItemRepository;
        }

        [Function("FeedFollowFunction")]
        public async Task Run([QueueTrigger("feedfollow", Connection = "StorageConnectionString")]string followId)
        {
            _logger.LogInformation($"{nameof(FeedFollowFunction)} started processing postId: {followId}");

            var objFollowId = new FollowId(followId);
            var posts = await _postRepository.GetAsync(objFollowId.StrFollowedProfileId);
            
            if((posts?.Any() ?? false) == true)
            {
                // TODO: Optimize using batch operations
                foreach(var post in posts)
                {
                    var feedItem = new FeedItem(objFollowId, post.PostId);
                    await _feedItemRepository.AddAsync(feedItem);
                }
            }

            _logger.LogInformation($"{nameof(FeedFollowFunction)} finished processing postId: {followId}");
        }
    }
}
