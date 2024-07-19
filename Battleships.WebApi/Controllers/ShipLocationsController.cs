using Battleships.Common.GameClasses;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipLocationsController(IShipLocationService shipLocationService, IGameStateService gameStateService) : ControllerBase
    {
        [HttpPost("user")]
        public IActionResult SetUserShips([FromBody] List<Ship> ships)
        {
            var gameState = gameStateService.GetGameState();
            gameState.UserShips = ships;
            gameStateService.SaveGameState(gameState);
            return Ok();
        }

        [HttpGet("opponent")]
        public ActionResult<List<Ship>> GetOpponentShips()
        {
            if (HttpContext.Session.GetString("SessionInitialized") == null)
            {
                HttpContext.Session.SetString("SessionInitialized", "true");
            }

            var gameState = gameStateService.GetGameState();

            gameState.OpponentShips = shipLocationService.GenerateOpponentShips();
            gameStateService.SaveGameState(gameState);

            return Ok(gameState.OpponentShips);
        }
    }
}