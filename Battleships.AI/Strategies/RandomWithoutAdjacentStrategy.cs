
using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies
{
    /// <summary>
    /// Random shots, but taking sunk ships into consideration - not shooting at cells adjacent to already sunk ships.
    /// </summary>
    public class RandomWithoutAdjacentStrategy : IAiStrategy
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

        private static bool IsNotAdjacentToSunkShip(int x, int y, GameState gameState)
        {
            var adjacentOffsets = new (int dx, int dy)[]
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),          (0, 1),
                (1, -1), (1, 0), (1, 1)
            };

            return adjacentOffsets.All(offset =>
            {
                var adjX = x + offset.dx;
                var adjY = y + offset.dy;

                if (adjX >= 0 && adjX < 10 && adjY >= 0 && adjY < 10)
                {
                    var adjacentShot = gameState.OpponentShots.FirstOrDefault(s => s.X == adjX && s.Y == adjY);
                    if (adjacentShot != null && adjacentShot.IsHit)
                    {
                        // Check if the hit cell is part of a sunk ship
                        return gameState.OpponentShips.Any(ship =>
                            ship.Coordinates.Any(coord => coord.X == adjX && coord.Y == adjY) &&
                            ship.IsSunk);
                    }
                }

                return true;
            });
        }

        private static bool IsCellAvailable(GameState gameState, int x, int y)
        {
            return IsNotAdjacentToSunkShip(x, y, gameState) && !gameState.OpponentShots.Any(s => s.X == x && s.Y == y);
        }
    }
}
