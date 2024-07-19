using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.Services.Interfaces
{
    public interface IGenerateMoveService
    {
        (int X, int Y) GenerateMove(GameState gameState, AiType aiType);
    }
}
