﻿using CheesyTot.AzureTables.SimpleIndex.Attributes;
using System.Runtime.Serialization;

namespace ReferenceSocialNetwork.Common.Data.Entities
{
    public class Profile : TableEntityBase
    {
        public Profile()
        { }

        public Profile(string userId, Guid profileId)
            : base(userId, profileId.ToString("N"))
        { }

        public string UserId => PartitionKey;

        [IgnoreDataMember]
        public Guid ProfileId => Guid.Parse(RowKey);

        [SimpleIndex]
        public string Handle { get; set; }

        public string DisplayName { get; set; }

        public string Bio { get; set; }
    }
}
