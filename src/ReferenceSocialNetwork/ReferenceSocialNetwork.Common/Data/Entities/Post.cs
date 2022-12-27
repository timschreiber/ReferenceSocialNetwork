using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Post : TableEntityBase
    {
        public Post()
        { }

        public Post(Guid profileId, DateTime postDate)
            : base(profileId.ToString("N"), EncodePostDate(postDate))
        { }

        [IgnoreDataMember]
        public Guid ProfileId => Guid.Parse(PartitionKey);

        [IgnoreDataMember]
        public DateTime PostDate => DecodePostDate(RowKey);

        public string PostId => $"{PartitionKey}{RowKey}";

        [SimpleIndex]
        public string ParentPostId { get; set; }

        public string Content { get; set; }

        public static string EncodePostDate(DateTime postDate) =>
            (DateTime.MaxValue.Ticks - postDate.Ticks).ToString("X").PadLeft(16, '0');

        public static DateTime DecodePostDate(string encodedPostDate) =>
            new DateTime(DateTime.MaxValue.Ticks - Convert.ToInt64(encodedPostDate, 16));

        public static string GetPostId(Guid profileId, DateTime postDate) =>
            $"{EncodePostDate(postDate)}{profileId:N}";

        public static ValueTuple<Guid, DateTime> ParsePostId(string postId) =>
            new ValueTuple<Guid, DateTime>(Guid.Parse(postId[16..]), DecodePostDate(postId[..16]));
    }
}
