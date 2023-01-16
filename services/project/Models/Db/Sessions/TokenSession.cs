using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;

namespace Models.Db.Sessions
{
    public class TokenSession : IdEntity
    {
        [MaxLength(36)]
        public string Token { get; set; }
        
        // определять уровень доступа к эндпоинтам (для того, чтобы не сломать старые мобилки)
        public bool HasFullAccess { get; set; }

        [ForeignKey(nameof(WorkerAccount))]
        public long WorkerAccountId { get; set; }

        public virtual WorkerAccount WorkerAccount { get; set; }

        public DateTime StartDate { get; set; }

        // Not null, because token has an expiration date
        public DateTime EndDate { get; set; }
        
        // является ли техническим (мобилка или сайт в будущем)
        public bool IsTechnical { get; set; }
    }
}