using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies
{
    /// <summary>
    /// Completely random shot strategy.
    /// </summary>
    public class RandomStrategy : IAiStrategy
    {
        private static readonly Random _random = new();

        public (int X, int Y) GenerateMove(GameState gameState)
        {
            var validMoves = GetValidMoves(gameState);

            if (validMoves.Count != 0)
            {
                var move = validMoves[_random.Next(validMoves.Count)];
                return move;
            }

            // Fallback to a random move if no valid moves found (shouldn't happen with a 10x10 grid)
            return new RandomStrategy().GenerateMove(gameState);
        }

        private static List<(int X, int Y)> GetValidMoves(GameState gameState)
        {
            var moves = new List<(int X, int Y)>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (IsCellAvailable(gameState, x, y))
                    {
                        moves.Add((x, y));
                    }
                }
            }

            return moves;
        }

        private static bool IsCellAvailable(GameState gameState, int x, int y)
        {
            return !gameState.OpponentShots.Any(s => s.X == x && s.Y == y);
        }
    }
}

