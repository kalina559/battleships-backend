using Battleships.Common.GameClasses;
using System.Diagnostics.Metrics;

namespace Battleships.Common.Helpers
{
    public static class GridHelper
    {
        public static IEnumerable<(int X, int Y)> GetAllAdjacentCells(int x, int y)
        {
            return GetSideAdjacentCells(x, y).Concat(GetEdgeAdjacentCells(x, y));
        }

        /// <summary>
        /// Gets all cells that are touching the sides of the cell with (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IEnumerable<(int X, int Y)> GetSideAdjacentCells(int x, int y)
        {
            return new List<(int X, int Y)>
    {
        (x - 1, y), (x + 1, y),
        (x, y - 1), (x, y + 1)
    };
        }

        /// <summary>
        /// Gets all cells that are touching the edges of the cell with (x,y) coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IEnumerable<(int X, int Y)> GetEdgeAdjacentCells(int x, int y)
        {
            return new List<(int X, int Y)>
    {
        (x - 1, y - 1), (x - 1, y + 1),
        (x + 1, y - 1), (x + 1, y + 1)
    };
        }

        public static bool IsValidShipPosition(GameState gameState, int x, int y, int length, bool isVertical)
        {
            int maxLength = isVertical ? x + length : y + length;
            if (maxLength > 10) return false;

            for (int i = 0; i < length; i++)
            {
                int currentX = isVertical ? x + i : x;
                int currentY = isVertical ? y : y + i;

                if (IsEdgeAdjacentCellHit(gameState, currentX, currentY))
                {
                    return false;
                }

                if (!(IsCellAvailable(gameState, currentX, currentY)
                      || (IsCellHit(gameState, currentX, currentY) && !IsPartOfSunkShip(currentX, currentY, gameState))))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsCellAvailable(GameState gameState, int x, int y)
        {
            var shot = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y);
            return shot == null;
        }

        public static bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < 10 && y >= 0 && y < 10;
        }

        public static bool IsPartOfSunkShip(int x, int y, GameState gameState)
        {
            return gameState.UserShips
                .Any(ship => ship.IsSunk && ship.Coordinates.Any(coord => coord.X == x && coord.Y == y));
        }        

        public static bool IsCellHit(GameState gameState, int x, int y)
        {
            var shot = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y);
            return shot != null && shot.IsHit;
        }

        public static bool IsEdgeAdjacentCellHit(GameState gameState, int x, int y)
        {
            var edgeAdjacentCells = GetEdgeAdjacentCells(x, y);
            var hit = edgeAdjacentCells.Any(cell => gameState.OpponentShots.Any(shot => cell.X == shot.X && cell.Y == shot.Y && shot.IsHit));

            return hit;
        }

        public static void PrintProbabilityGrid(int[] probabilityMap, int rows, int columns)
        {
            int[,] grid = new int[rows, columns];

            var mapElements = probabilityMap.Select((prob, index) => (prob, index)).ToList();

            foreach (var (probability, index) in mapElements)
            {

                int y = index / 10;
                int x = index % 10;
                grid[y, x] = probability;
            }

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Console.Write($"{grid[x, y],4}"); // Adjust spacing as needed
                }
                Console.WriteLine();
            }
        }

        public static void OrderHitCluster(ref List<(int X, int Y)> cluster)
        {
            cluster = [.. cluster.OrderBy(cell => cell.Y).ThenBy(cell => cell.X)];
        }
    }
}
