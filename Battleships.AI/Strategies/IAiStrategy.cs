using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies
{
    public interface IAiStrategy
    {
        (int X, int Y) GenerateMove(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch);
    }
}