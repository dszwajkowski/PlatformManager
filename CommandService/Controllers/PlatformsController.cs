using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("/api/c/[controller]")]
[ApiController]
public class PlatformsController : Controller
{
    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("Command service: Inbound POST");
        return Ok("Inbound test of from Platforms Controller");
    }
}
