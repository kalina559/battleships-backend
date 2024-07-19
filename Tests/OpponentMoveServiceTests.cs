using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Core.Services;
using Battleships.Common.Helpers;

namespace Battleships.UnitTests
{
    public class OpponentMoveServiceTests
    {
        private readonly GenerateMoveService _opponentMoveService;

        public OpponentMoveServiceTests()
        {
            _opponentMoveService = new GenerateMoveService();
        }

        [Fact]
        public void HeuristicStrategy_IgnoreCellsAdjacentToSunkShips()
        {
            var gameState = new GameState
            {
                OpponentAiType = AiType.LocationAndHitHeuristic,
                OpponentShots = [new() { X = 5, Y = 4, IsHit = true }, new Shot { X = 5, Y = 5, IsHit = true }, new Shot { X = 5, Y = 6, IsHit = true }],
                ShipsCanTouch = false,
                UserShips =
                [
                    new() { Size = 1, Coordinates = [new() { X = 0, Y = 0, IsHit = false }], IsSunk = false },
                    new() { Size = 2, Coordinates = [new() { X = 2, Y = 0, IsHit = false }, new() { X = 2, Y = 1, IsHit = false }], IsSunk = false },
                    new() { Size = 3, Coordinates = [new() { X = 5, Y = 4, IsHit = true }, new() { X = 5, Y = 5, IsHit = true }, new() { X = 5, Y = 6, IsHit = true }], IsSunk = true },
                    new() { Size = 4, Coordinates = [new() { X = 7, Y = 0, IsHit = false }, new() { X = 7, Y = 1, IsHit = false }, new() { X = 7, Y = 2, IsHit = false }, new() { X = 7, Y = 3, IsHit = false }], IsSunk = false },
                    new() { Size = 5, Coordinates = [new() { X = 9, Y = 0, IsHit = false }, new() { X = 9, Y = 1, IsHit = false }, new() { X = 9, Y = 2, IsHit = false }, new() { X = 9, Y = 3, IsHit = false }, new() { X = 9, Y = 4, IsHit = false }], IsSunk = false },
                ]
            };

            IEnumerable<(int, int)> forbiddenCells = GridHelper.GetAllAdjacentCells(5, 4).Concat(GridHelper.GetAllAdjacentCells(5, 5)).Concat(GridHelper.GetAllAdjacentCells(5, 6));

            var testFail = false;


            for (int i = 0; i < 100; i++)
            {
                var move = _opponentMoveService.GenerateMove(gameState.OpponentShots, gameState.UserShips, gameState.ShipsCanTouch, gameState.OpponentAiType);

                if (forbiddenCells.Contains(move))
                {
                    testFail = true;
                }
            }
            Assert.False(testFail);
        }
    }
}