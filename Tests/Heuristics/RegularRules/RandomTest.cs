﻿using Battleships.Common.GameClasses;
using Battleships.Core.Enums;

namespace Battleships.UnitTests.Heuristics.RegularRules
{
    public class RandomTest : AlgorithmTestBase
    {
        private readonly int numberOfIterations = 0;

        [Fact]
        public void VsRandomPlusShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
        public void VsLocationHeuristicExtendedShipsCanTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.Random,
                OpponentAiType = AiType.LocationHeuristicDynamic,
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
        public void VsHitHeuristicShipsCantTouch()
        {
            _httpContextAccessor.SetupTestHttpContext();

            var initialGameState = new GameState
            {
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
                PlayerAiType = AiType.Random,
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
