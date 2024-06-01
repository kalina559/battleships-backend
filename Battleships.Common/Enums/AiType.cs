namespace Battleships.Core.Enums
{
    public enum AiType
    {
        Random = 0, // completely random
        RandomPlus = 1,  // random, but excluding cells that are adjacent to already sunk ships
        Heuristic = 2
    }
}
