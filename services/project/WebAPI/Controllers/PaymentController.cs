using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("[controller]/[action]")]
[ResponseCache(NoStore = true, Duration = 0)]
public class PaymentController : Controller
{
    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Fail()
    {
        return View();
    }
}