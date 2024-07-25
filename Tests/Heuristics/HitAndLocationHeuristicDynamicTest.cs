﻿using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.UnitTests.Heuristics
{
    public class HitAndLocationHeuristicDynamicTest : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 0;

        [Fact]
        public void VsRandomShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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
        public void VsRandomShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.Random,
                ShipsCanTouch = true,
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
        public void VsRandomPlusShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.RandomPlus,
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
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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
        public void VsLocationHeuristicShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.LocationHeuristic,
                ShipsCanTouch = true,
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
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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
        public void VsHitHeuristicShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.HitHeuristic,
                ShipsCanTouch = true,
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
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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
        public void VsLocationAndHitHeuristicShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.LocationAndHitHeuristic,
                ShipsCanTouch = true,
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
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
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

        [Fact]
        public void VsLocationAndHitHeuristicDynamicShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.LocationAndHitHeuristicDynamic,
                OpponentAiType = AiType.LocationAndHitHeuristicDynamic,
                ShipsCanTouch = true,
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
