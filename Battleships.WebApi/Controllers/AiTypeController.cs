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
            new OutputAiInfo() { Type = AiType.Random, ShipsCanTouch = true, Description = new() { { "en", "Random shots" }, { "pl", "Losowe strzały" } } },
            new OutputAiInfo() { Type = AiType.RandomPlus, ShipsCanTouch = false, Description = new() { { "en", "Random shots excluding cells bordering sunk ships" }, { "pl", "Losowe strzały, z pominięciem pól sąsiadujących z zatopionymi statkami" } } },
            new OutputAiInfo() { Type = AiType.LocationHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Heuristic focused on cells that could be the location for the most ships" }, { "pl", "Heurystyka skupiona się na polach, które mogą być lokalizacją największej liczby statków" } } },
            new OutputAiInfo() { Type = AiType.LocationHeuristicDynamic, ShipsCanTouch = true, Description = new() { { "en", "Heuristic focused on cells that could be the location for biggest player's ship left" }, { "pl", "Heurystyka skupiona się na polach, które mogą być lokalizacją największego pozostałego statku użytkownika" } } },
            new OutputAiInfo() { Type = AiType.HitHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Heuristic focused on successful hits" }, { "pl", "Heurystyka skupiona na dotychczasowych trafieniach" } } },
            new OutputAiInfo() { Type = AiType.LocationAndHitHeuristic, ShipsCanTouch = true, Description = new() { { "en", "Most complete heuristic, analyzes possible locations and successful hits " }, { "pl", "Najbardziej kompletna heurystyka, analizująca możliwe lokalizacje statków oraz dotychczasowe trafienia" } } },
            new OutputAiInfo() { Type = AiType.LocationAndHitHeuristicDynamic, ShipsCanTouch = true, Description = new() { { "en", "Most complete heuristic, but prioritizes finiding longer ships first." }, { "pl", "Najbardziej kompletna heurystyka, ale priorytetyzuje odnalezienie najdłuższych statków." } } }

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