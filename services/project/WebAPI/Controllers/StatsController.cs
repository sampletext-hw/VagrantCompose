using Microsoft.AspNetCore.Mvc;
using Services.CommonServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    public class StatsController : Controller
    {
        private readonly IRequestCounterService _requestCounterService;
        
        public StatsController(IRequestCounterService requestCounterService)
        {
            _requestCounterService = requestCounterService;
        }

        [HttpGet]
        [SudoFilter]
        public ActionResult Index()
        {
            return View(_requestCounterService.Get());
        }
    }
}