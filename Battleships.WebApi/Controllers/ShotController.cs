using Battleships.Core.Common;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShotController(IGameStateService gameStateService, IOpponentMoveService opponentMoveService, ILogger<ShotController> logger) : ControllerBase
    {
        [HttpPost("user")]
        public IActionResult UserShot([FromBody] Shot shot)
        {
            var result = gameStateService.ProcessShot(shot.X, shot.Y, isPlayer: true);
            var win = gameStateService.CheckWinCondition();
            return Ok(new { result.IsHit, result.IsSunk, Win = win });
        }

        [HttpPost("opponent")]
        public IActionResult OpponentShot()
        {
            var (X, Y) = opponentMoveService.GenerateMove(gameStateService.GetGameState());

            var result = gameStateService.ProcessShot(X, Y, isPlayer: false);
            var win = gameStateService.CheckWinCondition();
            return Ok(new { result.IsHit, result.IsSunk, Win = win });
        }
    }
}
