using Battleships.Core.Common;
using Battleships.Services.Interfaces;

namespace Battleships.Core.Services
{
    public class OpponentMoveService : IOpponentMoveService
    {
        private static readonly Random _random = new();

        public (int X, int Y) GenerateMove(GameState gameState)
        {
            int x, y;
            bool isValidMove;

            do
            {
                x = _random.Next(10);
                y = _random.Next(10);
                isValidMove = !gameState.OpponentShots.Any(s => s.X == x && s.Y == y);
            } while (!isValidMove);

            return (x, y);
        }
    }
}
