using Battleships.Common.GameClasses;

namespace Battleships.AI.Strategies
{
    public interface IAiStrategy
    {
        (int X, int Y) GenerateMove(GameState gameState);
    }
}