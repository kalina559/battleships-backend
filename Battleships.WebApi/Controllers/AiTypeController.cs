using Battleships.AI.Strategies;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiTypeController(IAiTypeService aiTypeService) : Controller
    {
        private readonly List<OutputAiInfo> strategyInfo = [
            new OutputAiInfo() { Type = AiType.Random, ShipsCanTouch = true, Description = new() { { "en", "Random" }, { "pl", "Losowy" } } },
            new OutputAiInfo() { Type = AiType.RandomPlus, ShipsCanTouch = false, Description = new() { { "en", "Random extended" }, { "pl", "Losowy rozszerzony" } } },
            new OutputAiInfo() { Type = AiType.LocationHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Max. benefit from single shot" }, { "pl", "Maksymalizacja zysku ze strzału" } } },
            new OutputAiInfo() { Type = AiType.LocationHeuristicDynamic, ShipsCanTouch = true, Description = new() { { "en", "Max. benefit from single shot prioritizing biggest ships" }, { "pl", "Maksymalizacja zysku ze strzału priorytetyzująca większe statki" } } },
            new OutputAiInfo() { Type = AiType.HitHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Most probable location based on previous hits" }, { "pl", "Najbardziej prawdopodobna lokalizacja na podstawie trafień" } } },
            new OutputAiInfo() { Type = AiType.LocationAndHitHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Max. benefit from single shot + analyzing previous hits" }, { "pl", "Maksymalizacja zysku ze strzału + analiza poprzednich trafień" } } },
            new OutputAiInfo() { Type = AiType.LocationAndHitHeuristicDynamic, ShipsCanTouch = true, Description = new() { { "en", "Max. benefit from single shot prioritizing biggest ships + analyzing previous hits." }, { "pl", "Maksymalizacja zysku ze strzału priorytetyzująca większe statki + analiza poprzednich trafień" } } }

            ];
        
        [HttpPost("list")]
        public IActionResult GetAiTypes([FromBody] bool shipsCanTouch)
        {
            var types = strategyInfo
                .Where(x => x.ShipsCanTouch || (!x.ShipsCanTouch && !shipsCanTouch ));    // if shipsCanTouch == true and x.shipsCanTouch = false, the aiType is not available
            return Ok(types);
        }

        [HttpPost("select")]
        public IActionResult SelectAiType([FromBody] AiType type)
        {
            aiTypeService.SelectAiType(type);
            return Ok();
        }
    }

    public class OutputAiInfo
    {
        public required AiType Type { get; set; }
        public required bool ShipsCanTouch { get; set; }     // Indicates if this AiType works if ships touch
        public required Dictionary<string, string> Description { get; set; }
    }
}