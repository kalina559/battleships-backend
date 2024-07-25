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
    public class AlgorithmTestBase
    {
        protected Mock<ILogger<CosmosDbService>> _dbLogger = new();
        protected Mock<ILogger<GameStateService>> _gameStateLogger = new();
        protected readonly GenerateMoveService _generateMoveService;
        protected readonly ShipLocationService _shipLocationService;
        protected readonly GameStateService _gameStateService;
        protected readonly CosmosDbService _cosmosDbService;
        protected Mock<IHttpContextAccessor> _httpContextAccessor = new();
        protected Mock<IHostEnvironment> _environment = new();

        public AlgorithmTestBase()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            CosmosClient client = new(
               "https://kalj-cosmos-db.documents.azure.com:443/",
               "insertCosmosDbTokenHere"
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
    }
}