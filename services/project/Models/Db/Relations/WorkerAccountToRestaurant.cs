using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.DbRestaurant;

namespace Models.Db.Relations
{
    public class WorkerAccountToRestaurant
    {
        [ForeignKey(nameof(WorkerAccount))]
        public long WorkerAccountId { get; set; }

        public virtual WorkerAccount WorkerAccount { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}