using Battleships.Common.GameClasses;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RulesController(IRuleTypeService ruleTypeService) : Controller
    {
        [HttpPost("update")]
        public IActionResult UpdateRules([FromBody] bool shipsCanTouch)
        {
            ruleTypeService.SelectRuleType(shipsCanTouch);
            return Ok();
        }
    }
}