using Battleships.AI.Strategies;
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
                { AiType.RandomWithoutAdjacent, new RandomWithoutAdjacentStrategy() },
                { AiType.Heuristic, new HeuristicStrategy() }
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
