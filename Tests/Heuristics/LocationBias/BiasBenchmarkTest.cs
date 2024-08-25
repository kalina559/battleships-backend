using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.UnitTests.Heuristics.LocationBias
{
    public class BiasBenchmarkTest : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 1000;

        [Fact]
        public void ShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();


            AiType[] aiTypes = (AiType[])Enum.GetValues(typeof(AiType));

            var matchupCount = 0;

            for (int i = 0; i < aiTypes.Length; i++)
            {
                var initialGameState = new GameState
                {
                    PlayerAiType = aiTypes[i],
                    OpponentAiType = aiTypes[i],
                    ShipsCanTouch = false,
                    TestType = "BiasBenchmark"
                };

                matchupCount++;

                var playerWins = TestHelper.RunSimulation(
                    initialGameState,
                    _gameStateService,
                    _generateMoveService,
                    _shipLocationService,
                    numberOfIterations
                    );
            }

            Assert.Equal(7, matchupCount);
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
                var initialGameState = new GameState
                {
                    PlayerAiType = filteredAiTypes[i],
                    OpponentAiType = filteredAiTypes[i],
                    ShipsCanTouch = true,
                    TestType = "BiasBenchmark"
                };

                matchupCount++;

                var playerWins = TestHelper.RunSimulation(
                    initialGameState,
                    _gameStateService,
                    _generateMoveService,
                    _shipLocationService,
                    numberOfIterations
                    );
            }

            Assert.Equal(6, matchupCount);
        }
    }
}
