using Battleships.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Services
{
    public class ShipLocationService : IShipLocationService
    {
        private static readonly Random _random = new Random();

        public List<Ship> GenerateOpponentShips()
        {
            var ships = new List<Ship>
            {
                new Ship { Size = 5 },
                new Ship { Size = 4 },
                new Ship { Size = 3 },
                new Ship { Size = 2 },
                new Ship { Size = 1 }
            };

            var grid = new bool[10, 10];

            foreach (var ship in ships)
            {
                PlaceShip(ship, grid);
            }

            return ships;
        }

        private void PlaceShip(Ship ship, bool[,] grid)
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
                    if (IsAreaClear(grid, startX, startY, ship.Size, true))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startX + i, startY] = true;
                            MarkAdjacentCells(grid, startX + i, startY);
                            ship.Coordinates.Add(new Position { X = startX + i, Y = startY });
                        }
                        placed = true;
                    }
                }
                else if (direction == 1 && startY + ship.Size <= 10)
                {
                    // Vertical
                    if (IsAreaClear(grid, startX, startY, ship.Size, false))
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            grid[startX, startY + i] = true;
                            MarkAdjacentCells(grid, startX, startY + i);
                            ship.Coordinates.Add(new Position { X = startX, Y = startY + i });
                        }
                        placed = true;
                    }
                }
            }
        }

        private bool IsAreaClear(bool[,] grid, int startX, int startY, int size, bool horizontal)
        {
            for (int i = 0; i < size; i++)
            {
                int x = horizontal ? startX + i : startX;
                int y = horizontal ? startY : startY + i;

                if (grid[x, y] || IsAdjacentOccupied(grid, x, y))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAdjacentOccupied(bool[,] grid, int x, int y)
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

        private void MarkAdjacentCells(bool[,] grid, int x, int y)
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
