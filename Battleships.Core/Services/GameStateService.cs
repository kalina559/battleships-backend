using Battleships.Core.Common;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Battleships.Core.Services
{
    public class GameStateService(IHttpContextAccessor httpContextAccessor, ILogger<GameStateService> logger, IMemoryCache memoryCache) : IGameStateService
    {
        public GameState GetGameState()
        {
            var sessionId = httpContextAccessor.HttpContext.Request.Headers["X-Session-Id"].ToString();

            if (!memoryCache.TryGetValue(sessionId, out string gameStateJson))
            {
                logger.LogInformation("No game state found in cache for session ID: {sessionId}", sessionId);
                return new GameState();
            }

            logger.LogInformation("Game state retrieved from cache for session ID: {sessionId}", sessionId);
            return JsonConvert.DeserializeObject<GameState>(gameStateJson);
        }

        public void SaveGameState(GameState gameState)
        {
            var sessionId = httpContextAccessor.HttpContext.Request.Headers["X-Session-Id"].ToString();     // TODO move the header name to some const or config
            if (string.IsNullOrEmpty(sessionId))
            {
                logger.LogWarning("Session ID is missing in the request headers.");
                return;
            }

            var gameStateJson = JsonConvert.SerializeObject(gameState);
            memoryCache.Set(sessionId, gameStateJson);
            logger.LogInformation("Game state saved in cache for session ID: {sessionId}", sessionId);
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
                UserShips = [],
                OpponentShips = [],
                PlayerShots = [],
                OpponentShots = []
            };
            SaveGameState(gameState);
        }
    }
}