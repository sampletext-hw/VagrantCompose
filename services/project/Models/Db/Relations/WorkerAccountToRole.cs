using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;

namespace Models.Db.Relations
{
    public class WorkerAccountToRole
    {
        [ForeignKey(nameof(WorkerAccount))]
        public long WorkerAccountId { get; set; }

        public virtual WorkerAccount WorkerAccount { get; set; }

        [ForeignKey(nameof(WorkerRole))]
        public long WorkerRoleId { get; set; }

        public virtual WorkerRole WorkerRole { get; set; }
    }
}