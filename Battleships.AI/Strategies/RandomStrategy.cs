using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies
{
    /// <summary>
    /// Completely random shot strategy.
    /// </summary>
    public class RandomStrategy : IAiStrategy
    {
        private static readonly Random _random = new();

        public (int X, int Y) GenerateMove(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch)
        {
            var validMoves = GetValidMoves(previousShots);

            if (validMoves.Count != 0)
            {
                var move = validMoves[_random.Next(validMoves.Count)];
                return move;
            }

            // Fallback to a random move if no valid moves found (shouldn't happen with a 10x10 grid)
            return new RandomStrategy().GenerateMove(previousShots, opponentShips, shipsCanTouch);
        }

        private static List<(int X, int Y)> GetValidMoves(List<Shot> previousShots)
        {
            var moves = new List<(int X, int Y)>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (IsCellAvailable(previousShots, x, y))
                    {
                        moves.Add((x, y));
                    }
                }
            }

            return moves;
        }

        private static bool IsCellAvailable(List<Shot> previousShots, int x, int y)
        {
            return !previousShots.Any(s => s.X == x && s.Y == y);
        }
    }
}

