using Battleships.Core.Common;

namespace Battleships.Services.Interfaces
{
    public interface IOpponentMoveService
    {
        (int X, int Y) GenerateMove(GameState gameState);
    }
}
