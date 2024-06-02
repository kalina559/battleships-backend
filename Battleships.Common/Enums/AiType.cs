namespace Battleships.Core.Enums
{
    public enum AiType
    {
        Random = 0, // completely random
        RandomPlus = 1,  // random, but excluding cells that are adjacent to already sunk ships

        LocationHeuristic = 2,
        LocationHeuristicDynamic = 3,
        HitHeuristic = 4,
        LocationAndHitHeuristic = 5,
        LocationAndHitHeuristicDynamic = 6,
    }
}
