using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Follow : TableEntityBase
    {
        public Follow()
        { }

        public Follow(Guid followerProfileId, Guid followedProfileId)
            : base(followerProfileId.ToString("N"), followedProfileId.ToString("N"))
        { }

        [IgnoreDataMember]
        public Guid FollowerProfileId => Guid.Parse(PartitionKey);

        [IgnoreDataMember]
        public Guid FollowedProfileId => Guid.Parse(RowKey);

        public DateTime FollowDate { get; set; }
    }
}
