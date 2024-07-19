using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;

namespace Battleships.AI.Strategies.Heuristics
{
    /// <summary>
    /// Only take possible ship location into consideration. So the AI is shooting at cells, that are a possible location for the most ships.
    /// </summary>
    public class LocationHeuristicDynamicStrategy : HeuristicStrategyBase
    {
        private readonly static int POSSIBLE_SHIP_LOCATION_WEIGHT = 1;

        public override int[] GenerateProbabilityMap(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch)
        {
            var probabilityMap = new int[100];

            HeuristicHelper.AdjustProbabilityForShipLocations(previousShots, opponentShips, shipsCanTouch, probabilityMap, POSSIBLE_SHIP_LOCATION_WEIGHT, dynamicWeight: true, dynamicPower: 2);

            HeuristicHelper.AdjustProbabilityForShotAtCells(previousShots, probabilityMap);

            GridHelper.PrintProbabilityGrid(probabilityMap, 10, 10);    // just for debugging purposes

            return probabilityMap;
        }
    }
}
