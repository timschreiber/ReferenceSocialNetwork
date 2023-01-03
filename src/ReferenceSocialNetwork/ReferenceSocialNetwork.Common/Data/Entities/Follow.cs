using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Follow : TableEntityBase
    {
        public Follow()
        { }

        public Follow(FollowId followId)
            : base(followId.StrFollowerProfileId, followId.StrFollowedProfileId)
        { }

        [IgnoreDataMember]
        public Guid FollowerProfileId => Guid.Parse(PartitionKey);

        [IgnoreDataMember]
        [SimpleIndex]
        public Guid FollowedProfileId => Guid.Parse(RowKey);

        [IgnoreDataMember]
        public FollowId FollowId => new FollowId(PartitionKey, RowKey);

        public DateTime FollowDate { get; set; }
    }

    public class FollowId
    {
        private readonly string _followId;

        public FollowId(string followId) =>
            _followId = followId;

        public FollowId(Guid followerProfileId, Guid followedProfileId) =>
            _followId = $"{followerProfileId:N}{followedProfileId:N}";

        public FollowId(string strFollowerProfileid, string strFollowedProfileId) =>
            _followId = $"{strFollowerProfileid}{strFollowedProfileId}";

        public Guid FollowerProfileId => Guid.Parse(StrFollowerProfileId);
        public Guid FollowedProfileId => Guid.Parse(StrFollowedProfileId);
        public string StrFollowerProfileId => _followId[..16];
        public string StrFollowedProfileId => _followId[16..];
        public override string ToString() => _followId;
    }
}
