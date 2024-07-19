using Battleships.AI.Strategies;
using Battleships.AI.Strategies.Heuristics;
using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;

namespace Battleships.Core.Services
{
    public class GenerateMoveService : IGenerateMoveService
    {
        private readonly Dictionary<AiType, IAiStrategy> _strategies;

        public GenerateMoveService()
        {
            _strategies = new Dictionary<AiType, IAiStrategy>
            {
                { AiType.Random, new RandomStrategy() },
                { AiType.RandomPlus, new RandomWithoutAdjacentStrategy() },
                { AiType.LocationHeuristic, new LocationHeuristicStrategy() },
                { AiType.LocationHeuristicDynamic, new LocationHeuristicDynamicStrategy() },
                { AiType.HitHeuristic, new HitHeuristicStrategy() },
                { AiType.LocationAndHitHeuristic, new LocationAndHitHeuristicStrategy() },
                { AiType.LocationAndHitHeuristicDynamic, new LocationAndHitHeuristicDynamicStrategy() }
            };
        }

        public (int X, int Y) GenerateMove(GameState gameState, AiType aiType)
        {
            if (!_strategies.TryGetValue(aiType, out var strategy))
            {
                throw new ArgumentException($"Strategy for AI type {gameState.OpponentAiType} not found.");
            }

            return strategy.GenerateMove(gameState);
        }
    }
}
