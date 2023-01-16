using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Areas.Franchisee
{
    [Route("/franchisee/[controller]/[action]")]
    [TypeFilter(typeof(ValidateModelFilter))]
    [ResponseCache(NoStore = true, Duration = 0)]
    public class AkianaFranchiseeController : Controller
    {
    }
}