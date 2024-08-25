using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using System.Security.Cryptography;

namespace Battleships.UnitTests.Heuristics.ShipsCount
{
    public class MoreMiddleShips : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 1000;
        private readonly List<int> shipSizes = [5, 4, 3, 3, 3, 2, 1,];

        [Fact]
        public void ShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();


            AiType[] aiTypes = (AiType[])Enum.GetValues(typeof(AiType));

            var matchupCount = 0;

            for (int i = 0; i < aiTypes.Length; i++)
            {
                if (aiTypes[i] != AiType.LocationAndHitHeuristic)
                {
                    var initialGameState = new GameState
                    {
                        PlayerAiType = AiType.LocationAndHitHeuristic,
                        OpponentAiType = aiTypes[i],
                        ShipsCanTouch = false,
                        TestType = "MoreMiddleShips"
                    };

                    matchupCount++;

                    var playerWins = TestHelper.RunSimulation(
                        initialGameState,
                        _gameStateService,
                        _generateMoveService,
                        _shipLocationService,
                        numberOfIterations,
                        shipSizes);
                }
            }

            Assert.Equal(6, matchupCount);
        }

        [Fact]
        public void ShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            AiType[] aiTypes = (AiType[])Enum.GetValues(typeof(AiType));

            var filteredAiTypes = aiTypes.Except([AiType.RandomPlus]).ToArray();

            var matchupCount = 0;

            for (int i = 0; i < filteredAiTypes.Length; i++)
            {
                if (filteredAiTypes[i] != AiType.LocationAndHitHeuristic)
                {
                    var initialGameState = new GameState
                    {
                        PlayerAiType = AiType.LocationAndHitHeuristic,
                        OpponentAiType = filteredAiTypes[i],
                        ShipsCanTouch = true,
                        TestType = "MoreMiddleShips"
                    };

                    matchupCount++;

                    var playerWins = TestHelper.RunSimulation(
                        initialGameState,
                        _gameStateService,
                        _generateMoveService,
                        _shipLocationService,
                        numberOfIterations,
                        shipSizes);
                }
            }

            Assert.Equal(5, matchupCount);
        }       
    }
}
