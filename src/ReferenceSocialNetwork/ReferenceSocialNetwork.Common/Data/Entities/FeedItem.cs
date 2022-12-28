using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class FeedItem : TableEntityBase
    {
        public FeedItem()
        { }

        public FeedItem(Guid profileId, string postId)
            : base(profileId.ToString("N"), postId)
        { }

        [IgnoreDataMember]
        public Guid ProfileId => Guid.Parse(PartitionKey);

        [SimpleIndex]
        [IgnoreDataMember]
        public string PostId => RowKey;

        [SimpleIndex]
        [IgnoreDataMember]
        public Guid CreatorProfileId => Post.ParsePostId(RowKey).Item1;

        [SimpleIndex]
        [IgnoreDataMember]
        public string FollowId => $"{ProfileId:N}{CreatorProfileId:N}";

        [IgnoreDataMember]
        public DateTime PostDate => Post.ParsePostId(RowKey).Item2;
    }
}
