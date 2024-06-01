using Battleships.Common.Settings;
using Battleships.Core.Services;
using Microsoft.Azure.Cosmos;

namespace Battleships.WebApi
{
    public static class ServiceCollectionSetup
    {
        public static void InitializeCosmosClientInstanceAsync(CosmosDbSettings settings, IServiceCollection services)
        {
            CosmosClient client = new CosmosClient(settings.Account, settings.Key);
            services.AddSingleton(client);

            services.AddSingleton(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CosmosDbService>>();
                return new CosmosDbService(logger, settings, client);
            });
        }
    }
}
