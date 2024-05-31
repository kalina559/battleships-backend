using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AiTypeController(IAiTypeService aiTypeService) : Controller
{
    [HttpGet("list")]
    public IActionResult SelectAiType()
    {
        var types = aiTypeService
            .GetAllTypes()
            .Select(x => new { Id = (int)x, Name = x.ToString() });
        return Ok(types);
    }

    [HttpPost("select")]
    public IActionResult SelectAiType([FromBody] AiType type)
    {
        aiTypeService.SelectAiType(type);
        return Ok();
    }
}
