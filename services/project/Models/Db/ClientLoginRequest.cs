using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;

namespace Models.Db
{
    public class ClientLoginRequest : IdEntity
    {
        [ForeignKey(nameof(ClientAccount))]
        public long ClientAccountId { get; set; }

        public virtual ClientAccount ClientAccount { get; set; }

        [Range(1000, 10000)]
        public uint Code { get; set; }

        public DateTime IssuedAt { get; set; }

        public uint InvalidAttempts { get; set; }

        public bool IsResolved { get; set; }
        public Guid UniqueId { get; set; }
    }
}