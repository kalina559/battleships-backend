
using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;

namespace Battleships.AI.Strategies
{
    /// <summary>
    /// Random shots, but taking sunk ships into consideration - not shooting at cells adjacent to already sunk ships.
    /// </summary>
    public class RandomWithoutAdjacentStrategy : IAiStrategy
    {
        private static readonly Random _random = new();

        public (int X, int Y) GenerateMove(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch)
        {
            var validMoves = GetValidMoves(previousShots, opponentShips);

            if (validMoves.Count != 0)
            {
                var move = validMoves[_random.Next(validMoves.Count)];
                return move;
            }

            // Fallback to a random move if no valid moves found (shouldn't happen with a 10x10 grid)
            return new RandomStrategy().GenerateMove(previousShots, opponentShips, shipsCanTouch);
        }

        private static List<(int X, int Y)> GetValidMoves(List<Shot> previousShots, List<Ship> opponentShips)
        {
            var moves = new List<(int X, int Y)>();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (IsCellAvailable(previousShots, opponentShips, x, y))
                    {
                        moves.Add((x, y));
                    }
                }
            }

            return moves;
        }

        private static bool IsAdjacentToSunkShip(int x, int y, List<Shot> previousShots, List<Ship> opponentShips)
        {

            var adjacentCells = GridHelper.GetAllAdjacentCells(x, y);
            var sunkShips = opponentShips.Where(ship => ship.IsSunk);

            return adjacentCells.Any(
                cell => sunkShips.Any(
                    ship => ship.Coordinates.Any(
                        coord => coord.X == cell.X && coord.Y == cell.Y)));
        }

        private static bool IsCellAvailable(List<Shot> previousShots, List<Ship> opponentShips, int x, int y)
        {
            return !previousShots.Any(s => s.X == x && s.Y == y) && !IsAdjacentToSunkShip(x, y, previousShots, opponentShips) ;
        }
    }
}
