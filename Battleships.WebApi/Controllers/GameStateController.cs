using Battleships.Core.Common;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameStateController(IGameStateService gameStateService) : Controller
    {
        [HttpGet("get")]
        public IActionResult GetGameState()
        {
            var gameState = gameStateService.GetGameState();
            return Ok(gameState);
        }

        [HttpGet("clear")]
        public IActionResult ClearGameState()
        {
            gameStateService.ClearGameState();
            return Ok();
        }
    }
}