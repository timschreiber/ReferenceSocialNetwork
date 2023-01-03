using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Post : TableEntityBase
    {
        public Post()
        { }

        public Post(PostId postId)
            : base(postId.StrProfileId, postId.StrEncDateTime)
        { }

        [IgnoreDataMember]
        public Guid ProfileId => Guid.Parse(PartitionKey);

        public PostId PostId => new PostId(PartitionKey, RowKey);

        [IgnoreDataMember]
        public DateTime PostDate => PostId.PostDate;

        [SimpleIndex]
        public string ParentPostId { get; set; }

        public string Content { get; set; }
    }

    public class PostId
    {
        private readonly string _postId;

        public PostId(string postId) => _postId = postId;

        public PostId(Guid profileId, DateTime postDate)
        {
            var postDateVal = (DateTime.MaxValue.Ticks - postDate.Ticks).ToString("X").PadLeft(16, '0');
            _postId = $"{postDateVal}{profileId:N}";
        }

        public PostId(string strProfileId, string strEncPostDate) =>
            _postId = $"{strProfileId}{strEncPostDate}";

        public string StrProfileId => _postId[16..];
        public string StrEncDateTime => _postId[..16];
        public Guid ProfileId => Guid.Parse(StrProfileId);
        public DateTime PostDate => new DateTime(DateTime.MaxValue.Ticks - Convert.ToInt64(StrEncDateTime, 16));

        public override string ToString() => _postId;
    }
}
