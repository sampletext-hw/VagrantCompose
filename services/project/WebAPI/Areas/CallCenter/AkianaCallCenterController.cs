using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Areas.CallCenter
{
    // TODO: Remove this Route Attribute, it is only required for Swagger
    [Route("/callcenter/[controller]/[action]")]
    [TypeFilter(typeof(ValidateModelFilter))]
    [ResponseCache(NoStore = true, Duration = 0)]
    public abstract class AkianaCallCenterController : Controller
    {
    }
}