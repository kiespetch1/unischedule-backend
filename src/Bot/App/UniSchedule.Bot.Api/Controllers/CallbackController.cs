using Microsoft.AspNetCore.Mvc;
using UniSchedule.Bot.Entities;

namespace UniSchedule.Bot.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbackController(IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public IActionResult Callback([FromBody] Updates updates)
    {
        switch (updates.Type)
        {
            case "confirmation":
                return Ok(configuration["VkApiSettings:ConfirmationCode"]);
        }
        return Ok("ok");
    }
}