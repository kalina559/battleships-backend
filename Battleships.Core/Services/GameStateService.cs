using Battleships.Core.Common;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class GameStateService : IGameStateService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GameStateService> _logger;

    public GameStateService(IHttpContextAccessor httpContextAccessor, ILogger<GameStateService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public GameState GetGameState()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            _logger.LogWarning("HttpContext is null in GetGameState.");
            return new GameState();
        }

        var session = context.Session;
        var gameStateJson = session.GetString("GameState");
        if (string.IsNullOrEmpty(gameStateJson))
        {
            _logger.LogInformation("No game state found in session.");
            return new GameState();
        }

        _logger.LogInformation("Game state retrieved from session: {gameStateJson}", gameStateJson);
        return JsonConvert.DeserializeObject<GameState>(gameStateJson);
    }

    public void SaveGameState(GameState gameState)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            _logger.LogWarning("HttpContext is null in SaveGameState.");
            return;
        }

        var session = context.Session;
        var gameStateJson = JsonConvert.SerializeObject(gameState);
        session.SetString("GameState", gameStateJson);

        _logger.LogInformation("Game state saved to session: {gameStateJson}", gameStateJson);
    }

    public ShotResult ProcessShot(int x, int y, bool isPlayer)
    {
        var gameState = GetGameState();
        var targetShips = isPlayer ? gameState.OpponentShips : gameState.UserShips;
        var shotList = isPlayer ? gameState.PlayerShots : gameState.OpponentShots;

        var hit = targetShips.Any(ship => ship.Coordinates.Any(coord => coord.X == x && coord.Y == y));
        shotList.Add(new Shot { X = x, Y = y, IsHit = hit });
        bool isSunk = false;

        if (hit)
        {
            var ship = targetShips.First(s => s.Coordinates.Any(coord => coord.X == x && coord.Y == y));
            var shipSank = ship.Coordinates.All(coord => shotList.Any(shot => shot.X == coord.X && shot.Y == coord.Y));
            if (shipSank)
            {
                ship.IsSunk = true;
                isSunk = true;
            }
        }

        SaveGameState(gameState);

        return new ShotResult { IsHit = hit, IsSunk = isSunk };
    }

    public bool CheckWinCondition()
    {
        var gameState = GetGameState();

        var allPlayerShipsSunk = gameState.UserShips.All(ship => ship.IsSunk);
        var allOpponentShipsSunk = gameState.OpponentShips.All(ship => ship.IsSunk);

        return allPlayerShipsSunk || allOpponentShipsSunk;
    }

    public void ClearGameState()
    {
        var gameState = new GameState
        {
            UserShips = new List<Ship>(),
            OpponentShips = new List<Ship>(),
            PlayerShots = new List<Shot>(),
            OpponentShots = new List<Shot>()
        };
        SaveGameState(gameState);
    }
}
