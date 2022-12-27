using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Feed : TableEntityBase
    {
        public Feed()
        { }

        public Feed(Guid profileId, string postId)
            : base(profileId.ToString("N"), postId)
        { }

        [IgnoreDataMember]
        public Guid ProfileId => Guid.Parse(PartitionKey);

        [SimpleIndex]
        [IgnoreDataMember]
        public Guid CreatorProfileId => Post.ParsePostId(RowKey).Item1;

        [IgnoreDataMember]
        public DateTime PostDate => Post.ParsePostId(RowKey).Item2;
    }
}
