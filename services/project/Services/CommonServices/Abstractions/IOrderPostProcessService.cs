using System.Threading.Tasks;
using Models.Db.DbOrder;

namespace Services.CommonServices.Abstractions;

public interface IOrderPostProcessService
{
    Task PostProcessOrder(Order order);
}