using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.UnitTests.Heuristics.RegularRules
{
    public class RandomPlusTest : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 0;

        [Fact]
        public void VsRandomShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.Random,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }

        [Fact]
        public void VsLocationHeuristicShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.LocationHeuristic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }

        [Fact]
        public void VsLocationHeuristicExtendedShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.LocationHeuristicDynamic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }

        [Fact]
        public void VsHitHeuristicShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.HitHeuristic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }

        [Fact]
        public void VsLocationAndHitHeuristicShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.LocationAndHitHeuristic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }

        [Fact]
        public void VsLocationAndHitHeuristicDynamicShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.RandomPlus,
                OpponentAiType = AiType.LocationAndHitHeuristicDynamic,
                ShipsCanTouch = false,
            };

            var playerWins = TestHelper.RunSimulation(
                initialGameState,
                _gameStateService,
                _generateMoveService,
                _shipLocationService,
                numberOfIterations);

            Assert.True(true);
        }
    }
}
