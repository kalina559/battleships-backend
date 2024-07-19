using Battleships.Common.GameClasses;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShotController(IGameStateService gameStateService, IGenerateMoveService opponentMoveService) : ControllerBase
    {
        [HttpPost("user")]
        public IActionResult UserShot([FromBody] Shot shot)
        {
            var result = gameStateService.ProcessShot(shot.X, shot.Y, isPlayer: true);
            var win = gameStateService.CheckWinCondition();
            return Ok(new { result.IsHit, result.IsSunk, Win = win });
        }

        [HttpGet("opponent")]
        public IActionResult OpponentShot()
        {
            var gameState = gameStateService.GetGameState();
            var (X, Y) = opponentMoveService.GenerateMove(gameState.OpponentShots, gameState.UserShips, gameState.ShipsCanTouch, gameState.OpponentAiType);

            var result = gameStateService.ProcessShot(X, Y, isPlayer: false);
            var win = gameStateService.CheckWinCondition();
            return Ok(new { result.IsHit, result.IsSunk, Win = win });
        }
    }
}
