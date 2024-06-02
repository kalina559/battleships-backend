using Battleships.AI.Strategies;
using Battleships.AI.Strategies.Heuristics;
using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Services.Interfaces;

namespace Battleships.Core.Services
{
    public class OpponentMoveService : IOpponentMoveService
    {
        private readonly Dictionary<AiType, IAiStrategy> _strategies;

        public OpponentMoveService()
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

        public (int X, int Y) GenerateMove(GameState gameState)
        {
            if (!_strategies.TryGetValue(gameState.AiType, out var strategy))
            {
                throw new ArgumentException($"Strategy for AI type {gameState.AiType} not found.");
            }

            return strategy.GenerateMove(gameState);
        }
    }
}
