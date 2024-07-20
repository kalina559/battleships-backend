using Battleships.Common.CosmosDb;
using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task AddGameSessionAsync(GameSession gameSession, bool testMode);
    }
}
