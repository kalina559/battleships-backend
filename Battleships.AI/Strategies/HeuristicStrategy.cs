using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;
using System.Collections.Generic;

namespace Battleships.AI.Strategies
{
    public class HeuristicStrategy : IAiStrategy
    {
        private static readonly Random _random = new();

        private readonly static int POSSIBLE_SHIP_LOCATION_WEIGHT = 1;
        private readonly static int NEXT_TO_A_SINGLE_HIT_WEIGHT = 50;
        private readonly static int IN_LINE_WITH_OTHER_HITS_WEIGHT = 100;

        //X - vertical axis
        //Y - horizontal axis
        // TODO fix that

        public (int X, int Y) GenerateMove(GameState gameState)
        {
            var probabilityMap = GenerateProbabilityMap(gameState);

            var maxProbability = probabilityMap.Max();

            List<(int, int)> bestMoves = probabilityMap
                .Select((prob, index) => (prob, index))
                .Where(x => x.prob == maxProbability)
                .Select(x => (x.index % 10, x.index / 10))
                .Where(x => !gameState.OpponentShots.Any(shot => shot.X == x.Item1 && shot.Y == x.Item2))    //make sure we're not shooting at the same cell twice
                .ToList();

            if (bestMoves.Count != 0)
            {
                var move = bestMoves[_random.Next(bestMoves.Count)];
                return move;
            }

            // Fallback to a random move if no valid moves found (shouldn't happen with a 10x10 grid)
            return new RandomStrategy().GenerateMove(gameState);
        }

        private static int[] GenerateProbabilityMap(GameState gameState)
        {
            var probabilityMap = new int[100];

            AdjustProbabilityForShipLocations(gameState, probabilityMap);

            AdjustProbabilityForHitClusters(gameState, probabilityMap);

            AdjustProbabilityForSunkShips(gameState, probabilityMap);

            AdjustProbabilityForShotAtCells(gameState, probabilityMap);

            GridHelper.PrintProbabilityGrid(probabilityMap, 10, 10);    // just for debugging purposes

            return probabilityMap;
        }

        private static void AdjustProbabilityForShipLocations(GameState gameState, int[] probabilityMap)
        {
            var remainingShipLengths = gameState.UserShips
                .Where(ship => !ship.IsSunk)
                .Select(ship => ship.Coordinates.Count)
                .ToList();

            foreach (var length in remainingShipLengths)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (GridHelper.IsValidShipPosition(gameState, x, y, length, isVertical: true))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                probabilityMap[(y * 10) + x + i] += POSSIBLE_SHIP_LOCATION_WEIGHT;
                            }
                        }

                        if (GridHelper.IsValidShipPosition(gameState, x, y, length, isVertical: false))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                probabilityMap[((y + i) * 10) + x] += POSSIBLE_SHIP_LOCATION_WEIGHT;
                            }
                        }
                    }
                }
            }
        }        

        private static void AdjustProbabilityForHitClusters(GameState gameState, int[] probabilityMap)
        {            
            var hitClusters = FindHitClusters(gameState);
            foreach (var cluster in hitClusters)
            {
                if (cluster.Count > 1)
                {
                    var (startX, startY) = cluster.First();
                    var (endX, endY) = cluster.Last();

                    var isHorizontal = startX == endX;

                    if (isHorizontal)                    
                    {
                        // Increase probability for cells extending the horizontal cluster
                        IncreaseProbabilityForCell(gameState, startX, startY - 1, probabilityMap, IN_LINE_WITH_OTHER_HITS_WEIGHT);
                        IncreaseProbabilityForCell(gameState, endX, endY + 1, probabilityMap, IN_LINE_WITH_OTHER_HITS_WEIGHT);
                    }
                    else
                    {
                        // Increase probability for cells extending the vertical cluster
                        IncreaseProbabilityForCell(gameState, startX - 1, startY, probabilityMap, IN_LINE_WITH_OTHER_HITS_WEIGHT);
                        IncreaseProbabilityForCell(gameState, endX + 1, endY, probabilityMap, IN_LINE_WITH_OTHER_HITS_WEIGHT);
                    }
                }
                else
                {
                    AdjustProbabilityForSingleHit(gameState, probabilityMap, cluster.First());
                }
            }
        }

        private static void AdjustProbabilityForSingleHit(GameState gameState, int[] probabilityMap, (int X, int Y) hit)
        {
            var adjacentCells = GridHelper.GetSideAdjacentCells(hit.X, hit.Y);

            foreach (var (X, Y) in adjacentCells)
            {
                IncreaseProbabilityForCell(gameState, X, Y, probabilityMap, NEXT_TO_A_SINGLE_HIT_WEIGHT);
            }
        }

        private static void IncreaseProbabilityForCell(GameState gameState, int x, int y, int[] probabilityMap, int weight)
        {
            if (GridHelper.IsWithinBounds(x, y) && GridHelper.IsCellAvailable(gameState, x, y))
            {
                probabilityMap[y * 10 + x] += weight;
            }
        }

        private static List<List<(int X, int Y)>> FindHitClusters(GameState gameState)
        {
            var hitClusters = new List<List<(int X, int Y)>>();

            var visited = new bool[10, 10];

            foreach (var shot in gameState.OpponentShots.Where(s => s.IsHit && !GridHelper.IsPartOfSunkShip(s.X, s.Y, gameState)))
            {
                if (!visited[shot.X, shot.Y])
                {
                    var cluster = new List<(int X, int Y)>();
                    var queue = new Queue<(int X, int Y)>();

                    queue.Enqueue((shot.X, shot.Y));

                    while (queue.Count > 0)
                    {
                        var (cx, cy) = queue.Dequeue();

                        if (visited[cx, cy]) continue;

                        visited[cx, cy] = true;
                        cluster.Add((cx, cy));

                        foreach (var (X, Y) in GridHelper.GetSideAdjacentCells(cx, cy))
                        {
                            if (GridHelper.IsWithinBounds(X, Y) && !visited[X, Y])
                            {
                                var adjacentShot = gameState.OpponentShots.FirstOrDefault(s => s.X == X && s.Y == Y && s.IsHit);
                                if (adjacentShot != null && !GridHelper.IsPartOfSunkShip(adjacentShot.X, adjacentShot.Y, gameState))
                                {
                                    queue.Enqueue((X, Y));
                                }
                            }
                        }
                    }

                    if (cluster.Count > 0)
                    {
                        GridHelper.OrderHitCluster(ref cluster);
                        hitClusters.Add(cluster);
                    }
                }
            }
            
            return hitClusters;
        }

        /// <summary>
        /// If a ship was sunk, we can be sure that all adjacent cells contain no ships.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="probabilityMap"></param>
        private static void AdjustProbabilityForSunkShips(GameState gameState, int[] probabilityMap)
        {
            foreach (var ship in gameState.UserShips.Where(ship => ship.IsSunk))
            {
                foreach (var coord in ship.Coordinates)
                {
                    var adjacentCells = GridHelper.GetAllAdjacentCells(coord.X, coord.Y);
                    foreach (var (X, Y) in adjacentCells)
                    {
                        if (GridHelper.IsWithinBounds(X, Y))
                        {
                            probabilityMap[(Y * 10) + X] = 0;
                        }
                    }
                }
            }
        }

        private static void AdjustProbabilityForShotAtCells(GameState gameState, int[] probabilityMap)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (!GridHelper.IsCellAvailable(gameState, x, y))
                    {
                        probabilityMap[(y * 10) + x] = 0;
                    }
                }
            }
        }
    }
}
