﻿using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies.Heuristics
{
    public abstract class HeuristicStrategyBase : IAiStrategy
    {
        private static readonly Random _random = new();

        public (int X, int Y) GenerateMove(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch)
        {
            var probabilityMap = GenerateProbabilityMap(previousShots, opponentShips, shipsCanTouch);

            var maxProbability = probabilityMap.Max();

            List<(int, int)> bestMoves = probabilityMap
                .Select((prob, index) => (prob, index))
                .Where(x => x.prob == maxProbability)
                .Select(x => (x.index % 10, x.index / 10))
                .Where(x => !previousShots.Any(shot => shot.X == x.Item1 && shot.Y == x.Item2))    //make sure we're not shooting at the same cell twice
                .ToList();

            if (bestMoves.Count != 0)
            {
                var move = bestMoves[_random.Next(bestMoves.Count)];
                return move;
            }

            // Fallback to a random move if no valid moves found (shouldn't happen with a 10x10 grid)
            return new RandomStrategy().GenerateMove(previousShots, opponentShips, shipsCanTouch);
        }

        public abstract int[] GenerateProbabilityMap(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch);
    }
}
