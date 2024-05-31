using Battleships.Core.Common;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

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

        logger.LogInformation($"AiType updated to {type}");
    }
}
