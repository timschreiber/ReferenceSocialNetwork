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
    internal class FeedUnfollowFunction
    {
        private readonly ILogger _logger;

        public FeedUnfollowFunction(ILogger logger)
        {
            _logger = logger;
        }

        [Function("FeedUnfollowFunction")]
        public async Task Run([QueueTrigger("feedunfollow", Connection = "StorageConnectionString")]string followMessage)
        {
            _logger.LogInformation($"{nameof(FeedUnfollowFunction)} started processing profileId: {followMessage}");

            var strFollowerProfileId = followMessage[..32];
            var strFollowedProfileId = followMessage[32..];
            


            _logger.LogInformation($"{nameof(FeedUnfollowFunction)} started processing profileId: {followMessage}");
        }
    }
}
