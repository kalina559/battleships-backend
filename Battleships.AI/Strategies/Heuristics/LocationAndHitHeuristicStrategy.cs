using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;
using System.Collections.Generic;

namespace Battleships.AI.Strategies.Heuristics
{
    /// <summary>
    /// Takes both ship location and successful hits into consideration. Successful hits have a much higher priority.
    /// </summary>
    public class LocationAndHitHeuristicStrategy : HeuristicStrategyBase
    {
        private readonly static int POSSIBLE_SHIP_LOCATION_WEIGHT = 1;
        private readonly static int NEXT_TO_A_SINGLE_HIT_WEIGHT = 50;
        private readonly static int IN_LINE_WITH_OTHER_HITS_WEIGHT = 100;

        public override int[] GenerateProbabilityMap(GameState gameState)
        {
            var probabilityMap = new int[100];

            HeuristicHelper.AdjustProbabilityForShipLocations(gameState, probabilityMap, POSSIBLE_SHIP_LOCATION_WEIGHT);

            HeuristicHelper.AdjustProbabilityForHitClusters(gameState, probabilityMap, NEXT_TO_A_SINGLE_HIT_WEIGHT, IN_LINE_WITH_OTHER_HITS_WEIGHT);

            if (!gameState.ShipsCanTouch)
            {
                HeuristicHelper.AdjustProbabilityForSunkShips(gameState, probabilityMap);
            }

            HeuristicHelper.AdjustProbabilityForShotAtCells(gameState, probabilityMap);

            GridHelper.PrintProbabilityGrid(probabilityMap, 10, 10);    // just for debugging purposes

            return probabilityMap;
        }
    }
}
