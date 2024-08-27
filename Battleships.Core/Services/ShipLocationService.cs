using Battleships.Common.Enums;
using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;
using Battleships.Services.Interfaces;

namespace Battleships.Core.Services
{
    public class ShipLocationService(IGameStateService gameStateService) : IShipLocationService
    {
        private const double BiasProbability = 0.8;
        private const int CornerBiasRange = 3;
        private const int LeftBiasRange = 5;
        private const int CenterBiasRange = 6;

        private static readonly Random _random = new();

        readonly List<int> DefaultShipSizes = [5, 4, 3, 2, 1];

        public List<Ship> GenerateOpponentShips(List<int>? shipSizes = null, BiasType biasType = BiasType.None)
        {
            var sizesToPlace = shipSizes ?? DefaultShipSizes;

            var ships = new List<Ship>();

            foreach (var size in sizesToPlace)
            {
                ships.Add(new Ship { Size = size });
            }

            var grid = new bool[10, 10];
            var gameState = gameStateService.GetGameState();

            foreach (var ship in ships)
            {
                var success = PlaceShip(ship, grid, gameState.ShipsCanTouch, biasType);

                if (!success)
                {
                    // if we get 50 consecutive errors we just abandon this ship placing and try again
                    return GenerateOpponentShips(sizesToPlace);
                }
            }

            GridHelper.PrintGrid(grid, 10, 10);    // just for debugging purposes
            return ships;
        }

        private static bool PlaceShip(Ship ship, bool[,] grid, bool shipsCanTouch, BiasType biasType)
        {
            int errorCount = 0;
            bool placed = false;

            while (!placed)
            {

                if (errorCount > 50)
                {
                    GridHelper.PrintGrid(grid, 10, 10);    // just for debugging purposes
                    return false;
                }

                var direction = _random.Next(2); // 0: horizontal, 1: vertical

                var (startX, startY) = GetBiasedRandomPosition(10, ship.Size, biasType);

                if (direction == 0 && startX + ship.Size <= 10)
                {
                    // Horizontal
                    if (IsAreaClear(grid, startX, startY, ship.Size, true, shipsCanTouch))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startX + i, startY] = true;

                            if (!shipsCanTouch)
                            {
                                MarkAdjacentCells(grid, startX + i, startY);
                            }

                            ship.Coordinates.Add(new Coordinate { X = startX + i, Y = startY });
                        }
                        placed = true;
                    }
                }
                else if (direction == 1 && startY + ship.Size <= 10)
                {
                    // Vertical
                    if (IsAreaClear(grid, startX, startY, ship.Size, false, shipsCanTouch))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startX, startY + i] = true;

                            if (!shipsCanTouch)
                            {
                                MarkAdjacentCells(grid, startX, startY + i);
                            }

                            ship.Coordinates.Add(new Coordinate { X = startX, Y = startY + i });
                        }
                        placed = true;
                    }
                }

                errorCount++;
            }

            return true;
        }

        private static (int X, int Y) GetBiasedRandomPosition(int gridSize, int shipSize, BiasType biasType)
        {
            if (biasType != BiasType.None && _random.NextDouble() < BiasProbability)
            {
                switch (biasType)
                {
                    case BiasType.Corner:
                        return GenerateRandomCornerPosition(gridSize, shipSize);
                    case BiasType.Left:
                        return (_random.Next(0, LeftBiasRange), _random.Next(gridSize));
                    case BiasType.Center:
                        int centerStart = (gridSize / 2) - (CenterBiasRange / 2);
                        return (_random.Next(centerStart, centerStart + CenterBiasRange), _random.Next(centerStart, centerStart + CenterBiasRange));
                    default:
                        return (_random.Next(gridSize), _random.Next(gridSize));  // just as fallback
                }
            }
            else
            {
                return (_random.Next(gridSize), _random.Next(gridSize));
            }
        }

        private static (int X, int Y) GenerateRandomCornerPosition(int gridSize, int shipSize)
        {
            int corner = _random.Next(4);

            switch (corner)
            {
                case 0: // top-left
                    return (_random.Next(0, CornerBiasRange), _random.Next(0, CornerBiasRange));
                case 1: // top-right
                    return (_random.Next(gridSize - shipSize - CornerBiasRange, gridSize), _random.Next(0, CornerBiasRange));
                case 2: // bottom-left
                    return (_random.Next(0, CornerBiasRange), _random.Next(gridSize - shipSize - CornerBiasRange, gridSize));
                case 3: // bottom-right
                    return (_random.Next(gridSize - shipSize - CornerBiasRange, gridSize), _random.Next(gridSize - shipSize - CornerBiasRange, gridSize));
                default:
                    return (_random.Next(gridSize), _random.Next(gridSize));
            }
        }

        private static bool IsAreaClear(bool[,] grid, int startX, int startY, int size, bool horizontal, bool shipsCanTouch)
        {
            for (int i = 0; i < size; i++)
            {
                int x = horizontal ? startX + i : startX;
                int y = horizontal ? startY : startY + i;

                if (grid[x, y] || (!shipsCanTouch && IsAdjacentOccupied(grid, x, y)))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsAdjacentOccupied(bool[,] grid, int x, int y)
        {
            int[,] directions = {
        { -1, -1 }, { -1, 0 }, { -1, 1 },
        { 0, -1 },           { 0, 1 },
        { 1, -1 }, { 1, 0 }, { 1, 1 }
    };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10 && grid[newX, newY])
                {
                    return true;
                }
            }
            return false;
        }

        private static void MarkAdjacentCells(bool[,] grid, int x, int y)
        {
            int[,] directions = {
                {-1, -1}, {-1, 0}, {-1, 1},
                {0, -1},          {0, 1},
                {1, -1}, {1, 0}, {1, 1}
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
                {
                    grid[newX, newY] = true;
                }
            }
        }
    }
}
