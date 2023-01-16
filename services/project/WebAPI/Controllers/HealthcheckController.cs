using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers;

[Route("hc/[action]")]
public class HealthcheckController : Controller
{
    [HttpGet]
    [SwaggerOperation("для внутреннего использования")]
    public async Task<IActionResult> Check()
    {
        return Ok(1);
    }
}