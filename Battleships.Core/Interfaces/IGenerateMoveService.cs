using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.Services.Interfaces
{
    public interface IGenerateMoveService
    {
        (int X, int Y) GenerateMove(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch, AiType aiType);
    }
}
