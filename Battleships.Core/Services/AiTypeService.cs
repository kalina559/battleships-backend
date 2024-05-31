using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Battleships.Core.Services
{
    public class AiTypeService(ILogger<AiTypeService> logger, IGameStateService gameStateService) : IAiTypeService
    {
        public List<AiType> GetAllTypes()
        {
            var enumValues = Enum.GetValues(typeof(AiType))
                                     .Cast<AiType>()
                                     .ToList();

            return enumValues;
        }

        public void SelectAiType(AiType type)
        {
            var gameState = gameStateService.GetGameState();
            gameState.AiType = type;
            gameStateService.SaveGameState(gameState);

            logger.LogInformation("AiType updated to {type}.", type);
        }
    }
}
