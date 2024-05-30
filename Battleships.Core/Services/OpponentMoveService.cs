using Battleships.Core.Common;
using Battleships.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Core.Services
{
    public class OpponentMoveService : IOpponentMoveService
    {
        private static readonly Random _random = new Random();

        public (int X, int Y) GenerateMove(List<Position> userGrid, List<Shot> previousShots)
        {
            int x, y;
            bool isValidMove;

            do
            {
                x = _random.Next(10);
                y = _random.Next(10);
                isValidMove = !previousShots.Any(s => s.X == x && s.Y == y);
            } while (!isValidMove);

            bool isHit = userGrid.Any(pos => pos.X == x && pos.Y == y);

            previousShots.Add(new Shot { X = x, Y = y, IsHit = isHit });

            return (x, y);
        }
    }
}
