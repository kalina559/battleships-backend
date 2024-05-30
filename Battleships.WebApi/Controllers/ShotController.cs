using Battleships.Core.Common;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShotController(IGameStateService gameStateService, IOpponentMoveService opponentMoveService) : ControllerBase
{

    [HttpPost("user")]
    public ActionResult<ShotResult> UserShot([FromBody] Position position)
    {
        var gameState = gameStateService.GetGameState();
        var isHit = gameState.OpponentShips.Any(ship => ship.Coordinates.Any(coord => coord.X == position.X && coord.Y == position.Y));

        var shot = new Shot
        {
            X = position.X,
            Y = position.Y,
            IsHit = isHit
        };

        gameState.UserShots.Add(shot);
        gameStateService.SaveGameState(gameState);

        return Ok(new ShotResult { Position = position, IsHit = isHit });
    }

    [HttpGet("opponent")]
    public ActionResult<ShotResult> OpponentShot()
    {
        var gameState = gameStateService.GetGameState();
        var (x, y) = opponentMoveService.GenerateMove(gameState.UserShips.SelectMany(ship => ship.Coordinates).ToList(), gameState.OpponentShots);
        var isHit = gameState.UserShips.Any(ship => ship.Coordinates.Any(coord => coord.X == x && coord.Y == y));

        var shot = new Shot
        {
            X = x,
            Y = y,
            IsHit = isHit
        };

        gameState.OpponentShots.Add(shot);
        gameStateService.SaveGameState(gameState);

        return Ok(new ShotResult { Position = new Position { X = x, Y = y }, IsHit = isHit });
    }
}

public class ShotResult
{
    public required Position Position { get; set; }
    public bool IsHit { get; set; }
}
