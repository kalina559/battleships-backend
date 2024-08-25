using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using System.Security.Cryptography;

namespace Battleships.UnitTests.Heuristics.ShipsCount
{
    public class MoreSmallShipsTest : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 0;
        private readonly List<int> shipSizes = [5, 4, 3, 2, 1, 1, 1];

        [Fact]
        public void ShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();


            AiType[] aiTypes = (AiType[])Enum.GetValues(typeof(AiType));

            var matchupCount = 0;

            //for (int i = 0; i < aiTypes.Length; i++)
            //{
            //    for (int j = i + 1; j < aiTypes.Length; j++)
            //    {
            //        if (i != j)
            //        {
            //            var initialGameState = new GameState
            //            {
            //                PlayerAiType = aiTypes[i],
            //                OpponentAiType = aiTypes[j],
            //                ShipsCanTouch = false,
            //                ShipCountType = "ThreeOneCellShipsV2"
            //            };

            //            matchupCount++;

            //            var playerWins = TestHelper.RunSimulation(
            //                initialGameState,
            //                _gameStateService,
            //                _generateMoveService,
            //                _shipLocationService,
            //                numberOfIterations,
            //                shipSizes);
            //        }
            //    }
            //}

            for (int i = 0; i < aiTypes.Length; i++)
            {
                if (aiTypes[i] != AiType.LocationAndHitHeuristic)
                {
                    var initialGameState = new GameState
                    {
                        PlayerAiType = AiType.LocationAndHitHeuristic,
                        OpponentAiType = aiTypes[i],
                        ShipsCanTouch = false,
                        TestType = "ThreeOneCellShipsV2"
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
                        TestType = "ThreeOneCellShipsV2"
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
