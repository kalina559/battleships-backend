using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Core.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Battleships.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Moq;
using Microsoft.Extensions.Primitives;

namespace Battleships.UnitTests
{
    public class HeuristicTests
    {
        private Mock<ILogger<CosmosDbService>> _dbLogger = new();
        private Mock<ILogger<GameStateService>> _gameStateLogger = new();
        private readonly GenerateMoveService _opponentMoveService;
        private readonly ShipLocationService _shipLocationService;
        private readonly GameStateService _gameStateService;
        private readonly CosmosDbService _cosmosDbService;
        private Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private Mock<IMemoryCache> _memoryCache = new();
        private Mock<IHostEnvironment> _environment = new();


        public HeuristicTests()
        {
            CosmosClient client = new(
                "https://kalj-cosmos-db.documents.azure.com:443/",
                "__CosmosDb-Secret__"
                );
            CosmosDbSettings settings = new()
            {
                ContainerName = "test",
                DatabaseName = "test",
            };

            _opponentMoveService = new GenerateMoveService();
            _cosmosDbService = new CosmosDbService(_dbLogger.Object, settings, client);
            _gameStateService = new GameStateService(_httpContextAccessor.Object, _gameStateLogger.Object, _memoryCache.Object, _cosmosDbService, _environment.Object);
            _shipLocationService = new ShipLocationService(_gameStateService);
        }

        [Fact]
        public void RandomVsLocationHeuristic()
        {
            SetupHttpContext();

            var gameState = new GameState
            {
                OpponentAiType = AiType.LocationAndHitHeuristic,
                ShipsCanTouch = false,
            };            

            gameState.OpponentShips = _shipLocationService.GenerateOpponentShips();
            gameState.UserShips = _shipLocationService.GenerateOpponentShips();

            Assert.True(true);
        }

        private void SetupHttpContext()
        {
            var httpContextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var headersMock = new Mock<IHeaderDictionary>();

            headersMock.Setup(h => h["X-Session-Id"]).Returns(new StringValues("test"));

            requestMock.Setup(r => r.Headers).Returns(headersMock.Object);
            httpContextMock.Setup(ctx => ctx.Request).Returns(requestMock.Object);
            _httpContextAccessor.Setup(h => h.HttpContext).Returns(httpContextMock.Object);
        }
    }
}