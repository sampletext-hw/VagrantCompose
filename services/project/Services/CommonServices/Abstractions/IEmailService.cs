using System.Threading.Tasks;
using Models.Db.DbOrder;

namespace Services.CommonServices.Abstractions
{
    public interface IEmailService
    {
        public Task SendOrderTechEmail(Order order);
    }
}