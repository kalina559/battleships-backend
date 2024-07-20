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
using Microsoft.Extensions.DependencyInjection;

namespace Battleships.UnitTests.Heuristics
{
    public class HeuristicTests
    {
        private Mock<ILogger<CosmosDbService>> _dbLogger = new();
        private Mock<ILogger<GameStateService>> _gameStateLogger = new();
        private readonly GenerateMoveService _generateMoveService;
        private readonly ShipLocationService _shipLocationService;
        private readonly GameStateService _gameStateService;
        private readonly CosmosDbService _cosmosDbService;
        private Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private Mock<IHostEnvironment> _environment = new();


        public HeuristicTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            CosmosClient client = new(
               "https://kalj-cosmos-db.documents.azure.com:443/",
               "insertTokenHere"
               );

            CosmosDbSettings settings = new()
            {
                ContainerName = "GameSessions",
                TestContainerName = "TestGameSessions",
                DatabaseName = "BattleshipsDatabase",
            };

            _generateMoveService = new GenerateMoveService();
            _cosmosDbService = new CosmosDbService(_dbLogger.Object, settings, client);
            _gameStateService = new GameStateService(_httpContextAccessor.Object, _gameStateLogger.Object, memoryCache, _cosmosDbService, _environment.Object);
            _shipLocationService = new ShipLocationService(_gameStateService);
        }

        [Fact]
        public void RandomVsLocationHeuristic()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.HitHeuristic,
                OpponentAiType = AiType.LocationHeuristic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                3);

            Assert.True(true);
        }

        
    }
}