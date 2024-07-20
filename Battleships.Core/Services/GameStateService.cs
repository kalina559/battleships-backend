using Battleships.Common.CosmosDb;
using Battleships.Common.GameClasses;
using Battleships.Core.Exceptions;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Battleships.Core.Services
{
    public class GameStateService(IHttpContextAccessor httpContextAccessor, ILogger<GameStateService> logger, IMemoryCache memoryCache, ICosmosDbService cosmosDbService, IHostEnvironment env) : IGameStateService
    {
        public GameState GetGameState()
        {
            var sessionId = httpContextAccessor.HttpContext.Request.Headers["X-Session-Id"].ToString();

            if (!memoryCache.TryGetValue(sessionId, out string? gameStateJson))
            {
                logger.LogInformation("No game state found in cache for session ID: {sessionId}", sessionId);
                return new GameState();
            }           

            var deserializedGameState = DeserializeGameStateJson(gameStateJson);

            logger.LogInformation("Game state retrieved from cache for session ID: {sessionId}", sessionId);
            return deserializedGameState;
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

        public bool CheckWinCondition(bool testMode = false)
        {
            var gameState = GetGameState();

            var allPlayerShipsSunk = gameState.UserShips.All(ship => ship.IsSunk);
            var allOpponentShipsSunk = gameState.OpponentShips.All(ship => ship.IsSunk);

            if (allPlayerShipsSunk || allOpponentShipsSunk)
            {
                SaveGameSessionToDb(gameState, playerWon: allOpponentShipsSunk, testMode);
                return true;
            }

            return false;
        }

        private void SaveGameSessionToDb(GameState gameState, bool playerWon, bool testMode)
        {
            if (env.IsProduction() || testMode)
            {
                var gameSession = new GameSession
                {
                    Id = Guid.NewGuid(),
                    GameStateJson = JsonConvert.SerializeObject(gameState),
                    SessionId = httpContextAccessor.HttpContext.Request.Headers["X-Session-Id"].ToString(),
                    DateCreated = DateTime.UtcNow,
                    PlayerAiType = (int?)gameState.PlayerAiType,
                    OpponentAiType = (int)gameState.OpponentAiType,
                    ShipsCanTouch = gameState.ShipsCanTouch,
                    OpponentShipsSunk = gameState.OpponentShips.Where(x => x.IsSunk).Count(),
                    PlayersShipsSunk = gameState.UserShips.Where(x => x.IsSunk).Count(),
                    OpponentMovesCount = gameState.OpponentShots.Count(),
                    PlayerMovesCount = gameState.PlayerShots.Count(),
                    PlayerWon = playerWon
                };

                cosmosDbService.AddGameSessionAsync(gameSession, testMode);
            }
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

        private static GameState DeserializeGameStateJson(string? gameStateJson)
        {
            if (gameStateJson == null)
            {
                throw new NullGameStateException($"Game state JSON was found but was null.");
            }

            var gameState = JsonConvert.DeserializeObject<GameState>(gameStateJson);

            return gameState ?? throw new NullGameStateException($"Game state JSON was found but was null after deserialization");
        }
    }
}