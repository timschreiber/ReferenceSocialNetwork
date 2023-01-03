using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class FeedItem : TableEntityBase
    {
        public FeedItem()
        { }

        public FeedItem(FollowId followId, PostId postId)
            : base(followId.ToString(), postId.ToString())
        { }

        [IgnoreDataMember]
        public FollowId FollowId => new(PartitionKey);

        [IgnoreDataMember]
        [SimpleIndex]
        public Guid FollowerProfileId => FollowId.FollowerProfileId;

        [IgnoreDataMember]
        [SimpleIndex]
        public Guid FollowedProfileId => PostId.ProfileId;

        [IgnoreDataMember]
        [SimpleIndex]
        public PostId PostId => new(RowKey);

        [IgnoreDataMember]
        public DateTime PostDate => PostId.PostDate;
    }
}
