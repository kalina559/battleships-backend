using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;

namespace Battleships.AI.Strategies.Heuristics
{
    /// <summary>
    /// Only take succesful shots into consideration. So the AI is shooting at cells that are next to successful hits, or are extending a cluster of hits.
    /// </summary>
    public class HitHeuristicStrategy : HeuristicStrategyBase
    {
        private readonly static int
            NEXT_TO_A_SINGLE_HIT_WEIGHT = 50,
            IN_LINE_WITH_OTHER_HITS_WEIGHT = 100;

        public override int[] GenerateProbabilityMap(List<Shot> previousShots, List<Ship> opponentShips, bool shipsCanTouch)
        {
            var probabilityMap = new int[100];

            HeuristicHelper.AdjustProbabilityForHitClusters(previousShots, opponentShips, probabilityMap, NEXT_TO_A_SINGLE_HIT_WEIGHT, IN_LINE_WITH_OTHER_HITS_WEIGHT);

            if (shipsCanTouch)
            {
                HeuristicHelper.AdjustProbabilityForSunkShips(opponentShips, probabilityMap);
            }

            HeuristicHelper.AdjustProbabilityForShotAtCells(previousShots, probabilityMap);

            GridHelper.PrintProbabilityGrid(probabilityMap, 10, 10);    // just for debugging purposes

            return probabilityMap;
        }
    }
}
