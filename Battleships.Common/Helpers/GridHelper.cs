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

        public static bool IsCellEmpty(GameState gameState, int x, int y) 
        {
            var shot = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y);
            return shot == null;
        }

        public static bool IsCellHit(GameState gameState, int x, int y)
        {
            var shot = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y);
            return shot == null || shot.IsHit;
        }

        public static bool IsVerticalNeighbourHit(GameState gameState, int x, int y)
        {
            var first = gameState.OpponentShots.FirstOrDefault(s => s.X == x - 1 && s.Y == y);
            var second = gameState.OpponentShots.FirstOrDefault(s => s.X == x + 1 && s.Y == y);

            return first != null || second != null;
        }

        public static bool IsHorizontalNeighbourHit(GameState gameState, int x, int y)
        {
            var first = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y - 1);
            var second = gameState.OpponentShots.FirstOrDefault(s => s.X == x && s.Y == y + 1);

            return first != null || second != null;
        }
    }
}
