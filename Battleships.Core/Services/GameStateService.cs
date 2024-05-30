using Battleships.Core.Common;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Battleships.Core
{
    public class GameStateService(ILogger<GameStateService> logger, IHttpContextAccessor httpContextAccessor) : IGameStateService
    {
        public GameState GetGameState()
        {
            var session = httpContextAccessor.HttpContext.Session;
            var gameStateJson = session.GetString("GameState");

            return string.IsNullOrEmpty(gameStateJson) ? new GameState() : JsonConvert.DeserializeObject<GameState>(gameStateJson);
        }

        public void SaveGameState(GameState gameState)
        {
            var session = httpContextAccessor.HttpContext.Session;
            var gameStateJson = JsonConvert.SerializeObject(gameState);
            session.SetString("GameState", gameStateJson);
        }
    }
}
