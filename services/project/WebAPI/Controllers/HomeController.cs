using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Models.DTOs.Misc;
using Models.Misc;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Test()
        {
            throw new AkianaException("Example exception");
            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(SerializeOutputFilter))]
        public IActionResult Encrypt([FromBody] object obj)
        {
            return Ok(obj);
        }

        [HttpPost]
        [TypeFilter(typeof(ValidateModelFilter))]
        public IActionResult Decrypt([ModelBinder(typeof(EncodedJsonBinder))] object obj)
        {
            return Ok(obj);
        }
    }
}