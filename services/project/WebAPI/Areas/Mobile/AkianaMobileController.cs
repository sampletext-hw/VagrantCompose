using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile
{
    // TODO: Remove this Route Attribute, it is only required for Swagger
    [Route("/m/[controller]/[action]/")]
    [TypeFilter(typeof(ValidateModelFilter))]
    [ResponseCache(NoStore = true, Duration = 0)]
    public abstract class AkianaMobileController : Controller
    {
    }
}