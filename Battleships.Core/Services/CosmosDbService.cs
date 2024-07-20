using Battleships.Common.CosmosDb;
using Battleships.Common.Settings;
using Battleships.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Battleships.Core.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;
        private readonly Container _testContainer;
        private readonly ILogger<CosmosDbService> _logger;

        public CosmosDbService(ILogger<CosmosDbService> logger, CosmosDbSettings settings, CosmosClient dbClient)
        {
            _logger = logger;
            _container = dbClient.GetContainer(settings.DatabaseName, settings.ContainerName);
            _testContainer = dbClient.GetContainer(settings.DatabaseName, settings.TestContainerName);
        }

        public async Task AddGameSessionAsync(GameSession gameSession, bool testMode)
        {
            if (testMode)
            {
                await _testContainer.CreateItemAsync(gameSession, new PartitionKey(gameSession.SessionId));
            }
            else
            {
                await _container.CreateItemAsync(gameSession, new PartitionKey(gameSession.SessionId));
            }
        }
    }
}