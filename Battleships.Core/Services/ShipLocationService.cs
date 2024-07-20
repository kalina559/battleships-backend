using Battleships.Common.GameClasses;
using Battleships.Common.Helpers;
using Battleships.Services.Interfaces;

namespace Battleships.Core.Services
{
    public class ShipLocationService(IGameStateService gameStateService) : IShipLocationService
    {
        private static readonly Random _random = new();

        static int errors = 0;

        public List<Ship> GenerateOpponentShips()
        {
            var ships = new List<Ship>
            {
                new() { Size = 5 },
                new() { Size = 4 },
                new() { Size = 3 },
                new() { Size = 2 },
                new() { Size = 1 }
            };

            var grid = new bool[10, 10];
            var gameState = gameStateService.GetGameState();

            foreach (var ship in ships)
            {
                PlaceShip(ship, grid, gameState.ShipsCanTouch);
            }

            return ships;
        }

        private static void PlaceShip(Ship ship, bool[,] grid, bool shipsCanTouch)
        {
            bool placed = false;

            while (!placed)
            {
                var direction = _random.Next(2); // 0: horizontal, 1: vertical
                var startX = _random.Next(10);
                var startY = _random.Next(10);

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
                    if (errors > 50)
                    {
                        GridHelper.PrintGrid(grid, 10, 10);    // just for debugging purposes
                    }

                    errors++;
                    return false;
                }
            }
            errors = 0;
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
