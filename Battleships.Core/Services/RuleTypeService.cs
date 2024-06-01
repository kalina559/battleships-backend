using Battleships.Common.GameClasses;
using Battleships.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Battleships.Core.Services
{
    public class RuleTypeService(ILogger<RuleTypeService> logger, IGameStateService gameStateService) : IRuleTypeService
    {
        public void SelectRuleType(bool shipsCanTouch)
        {
            var gameState = gameStateService.GetGameState();
            gameState.ShipsCanTouch = shipsCanTouch;
            gameStateService.SaveGameState(gameState);

            logger.LogInformation("ShipsCanTouch updated to {type}.", shipsCanTouch);
        }
    }
}
