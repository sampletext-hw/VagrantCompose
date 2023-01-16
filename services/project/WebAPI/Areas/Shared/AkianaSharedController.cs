using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Db.Account;
using Services;
using WebAPI.Filters;

namespace WebAPI.Areas.Shared
{
    [Route("/shared/[controller]/[action]")]
    [TypeFilter(typeof(ValidateModelFilter))]
    [ResponseCache(NoStore = true, Duration = 0)]
    public class AkianaSharedController : Controller
    {
    }
}