using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.Db.Relations;

namespace Models.Db.Account
{
    public class WorkerRole : IdEntity
    {
        [MaxLength(64)]
        public string TitleEn { get; set; }

        [MaxLength(64)]
        public string TitleRu { get; set; }

        public virtual ICollection<WorkerAccount> WorkerAccounts { get; set; }
        
        public virtual ICollection<WorkerAccountToRole> WorkerAccountsRelation { get; set; }
    }
}