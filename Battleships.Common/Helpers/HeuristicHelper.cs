using Battleships.Common.GameClasses;

namespace Battleships.Common.Helpers
{
    public static class HeuristicHelper
    {
        public static void AdjustProbabilityForShipLocations(GameState gameState, int[] probabilityMap, int weight, bool dynamicWeight = false, int dynamicPower = 1)
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
                                probabilityMap[(y * 10) + x + i] += dynamicWeight ? weight * (int)Math.Pow(length, dynamicPower) : weight;
                            }
                        }

                        if (GridHelper.IsValidShipPosition(gameState, x, y, length, isVertical: false))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                probabilityMap[((y + i) * 10) + x] += dynamicWeight ? weight * (int)Math.Pow(length, dynamicPower) : weight;
                            }
                        }
                    }
                }
            }
        }

        public static int AdjustProbabilityForHitClusters(GameState gameState, int[] probabilityMap, int singleHitWeight, int clusterWeight)
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
                        IncreaseProbabilityForCell(gameState, startX, startY - 1, probabilityMap, clusterWeight);
                        IncreaseProbabilityForCell(gameState, endX, endY + 1, probabilityMap, clusterWeight);
                    }
                    else
                    {
                        // Increase probability for cells extending the vertical cluster
                        IncreaseProbabilityForCell(gameState, startX - 1, startY, probabilityMap, clusterWeight);
                        IncreaseProbabilityForCell(gameState, endX + 1, endY, probabilityMap, clusterWeight);
                    }
                }
                else
                {
                    AdjustProbabilityForSingleHit(gameState, probabilityMap, cluster.First(), singleHitWeight);
                }
            }

            return hitClusters.Count();
        }

        public static List<List<(int X, int Y)>> FindHitClusters(GameState gameState)
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

        public static void AdjustProbabilityForSingleHit(GameState gameState, int[] probabilityMap, (int X, int Y) hit, int weight)
        {
            var adjacentCells = GridHelper.GetSideAdjacentCells(hit.X, hit.Y);

            foreach (var (X, Y) in adjacentCells)
            {
                IncreaseProbabilityForCell(gameState, X, Y, probabilityMap, weight);
            }
        }
        public static void IncreaseProbabilityForCell(GameState gameState, int x, int y, int[] probabilityMap, int weight)
        {
            if (GridHelper.IsWithinBounds(x, y) && GridHelper.IsCellAvailable(gameState, x, y))
            {
                probabilityMap[y * 10 + x] += weight;
            }
        }

        public static void AdjustProbabilityForShotAtCells(GameState gameState, int[] probabilityMap)
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

        /// <summary>
        /// If a ship was sunk, we can be sure that all adjacent cells contain no ships.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="probabilityMap"></param>
        public static void AdjustProbabilityForSunkShips(GameState gameState, int[] probabilityMap)
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
    }
}
