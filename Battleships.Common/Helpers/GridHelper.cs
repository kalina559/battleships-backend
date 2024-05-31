using Battleships.Common.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Common.Helpers
{
    public static class GridHelper
    {
        public static IEnumerable<(int X, int Y)> GetAllAdjacentCells(int x, int y)
        {
            return new List<(int X, int Y)>
    {
        (x - 1, y), (x + 1, y),
        (x, y - 1), (x, y + 1),
        (x - 1, y - 1), (x - 1, y + 1),
        (x + 1, y - 1), (x + 1, y + 1)
    };
        }

        public static IEnumerable<(int X, int Y)> GetSideAdjacentCells(int x, int y)
        {
            return new List<(int X, int Y)>
    {
        (x - 1, y), (x + 1, y),
        (x, y - 1), (x, y + 1)
    };
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

        public static bool IsVerticalNeighbourHit(GameState gameState, int x, int y)
        {
            var up = gameState.OpponentShots.FirstOrDefault(s => s.X == x - 1 && s.Y == y && s.IsHit);
            var down = gameState.OpponentShots.FirstOrDefault(s => s.X == x + 1 && s.Y == y && s.IsHit);

            return up != null || down != null;
        }

        public static bool IsHorizontalNeighbourCluster(GameState gameState, int x, int y)
        {
            var firstLeft = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y - 1 && s.IsHit);
            var secondLeft = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y - 2 && s.IsHit);

            var firstRight = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y + 1 && s.IsHit);
            var secondRight = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y + 2 && s.IsHit);


            return (firstLeft != null && secondLeft != null) || (firstRight != null && secondRight != null);
        }

        public static bool IsVerticalNeighbourCluster(GameState gameState, int x, int y)
        {
            var firstUp = gameState.OpponentShots.FirstOrDefault(s => s.X == x - 1 && s.Y == y && s.IsHit);
            var secondUp = gameState.OpponentShots.FirstOrDefault(s => s.X == x - 2 && s.Y == y && s.IsHit);

            var firstDown = gameState.OpponentShots.FirstOrDefault(s => s.X == x + 1 && s.Y == y && s.IsHit);
            var secondDown = gameState.OpponentShots.FirstOrDefault(s => s.X == x + 2 && s.Y == y && s.IsHit);

            return (firstUp != null && secondUp != null) || (firstDown != null && secondDown != null);
        }

        //public static bool IsShipInFrontHorizontal(GameState gameState, int x, int y)
        //{
        //    var front = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y && s.IsHit);
        //    var left = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y - 1 && s.IsHit);
        //    var right = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y + 1 && s.IsHit);


        //    return front != null && (left != null || right != null);
        //}

        //public static bool IsShipInFrontVertical(GameState gameState, int x, int y)
        //{
        //    var front = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y && s.IsHit);
        //    var up = gameState.OpponentShots.FirstOrDefault(s => s.X == x - 1 && s.Y == y && s.IsHit);
        //    var down = gameState.OpponentShots.FirstOrDefault(s => s.X == x + 1 && s.Y == y && s.IsHit);

        //    return front != null && (up != null || down != null);
        //}


        public static bool IsHorizontalNeighbourHit(GameState gameState, int x, int y)
        {
            var left = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y - 1 && s.IsHit);
            var right = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y + 1 && s.IsHit);

            return left != null || right != null;
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
    }
}
