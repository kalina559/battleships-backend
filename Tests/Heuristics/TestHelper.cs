using Battleships.Common.Enums;
using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Core.Services;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Battleships.UnitTests.Heuristics
{
    public static class TestHelper
    {
        public static void SetupTestHttpContext(this Mock<IHttpContextAccessor> accessor)
        {
            var httpContextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var headersMock = new Mock<IHeaderDictionary>();

            headersMock.Setup(h => h["X-Session-Id"]).Returns(new StringValues("test"));

            requestMock.Setup(r => r.Headers).Returns(headersMock.Object);
            httpContextMock.Setup(ctx => ctx.Request).Returns(requestMock.Object);
            accessor.Setup(h => h.HttpContext).Returns(httpContextMock.Object);
        }

        public static int RunSimulation(
            GameState initialGameState,
            GameStateService gameStateService,
            GenerateMoveService generateMoveService,
            ShipLocationService shipLocationService,
            int runs,
            List<int>? shipSizes = null,
            BiasType biasType = BiasType.None
            )
        {
            int playerWins = 0;

            for (int i = 0; i< runs; i++)
            {
                using var gameStateClone = initialGameState.Clone();
                Play(gameStateClone, gameStateService, generateMoveService, shipLocationService, ref playerWins, biasType, shipSizes);
            }

            return playerWins;
        }

        public static void Play(
            GameState initialGameState,
            GameStateService gameStateService,
            GenerateMoveService generateMoveService,
            ShipLocationService shipLocationService,
            ref int playerWins,
            BiasType biasType,
            List<int>? shipSizes = null     
            )
        {
            initialGameState.OpponentShips = shipLocationService.GenerateOpponentShips(shipSizes, biasType);
            initialGameState.UserShips = shipLocationService.GenerateOpponentShips(shipSizes);
            gameStateService.SaveGameState(initialGameState);

            bool gameIsFinished = false;
            int turnNumber = 1;
            int whoStarts = new Random().Next(0, 2);    // 0 - opponent has the first move, 1 - player has the first move

            while (!gameIsFinished)
            {
                var gameState = gameStateService.GetGameState();

                var playerTurn = (turnNumber + whoStarts) % 2 == 0;

                var targetShips = playerTurn
                    ? gameState.OpponentShips
                    : gameState.UserShips;

                var previousShots = playerTurn
                    ? gameState.PlayerShots
                    : gameState.OpponentShots;

                var aiType = playerTurn
                    ? gameState.PlayerAiType
                    : gameState.OpponentAiType;

                var move = generateMoveService.GenerateMove(previousShots, targetShips, gameState.ShipsCanTouch, (AiType)aiType);
                gameStateService.ProcessShot(move.X, move.Y, playerTurn);

                if (gameStateService.CheckWinCondition(testMode: true))
                {
                    if (playerTurn)
                    {
                        ++playerWins;
                    }

                    gameIsFinished = true;
                    gameStateService.ClearGameState();
                }

                ++turnNumber;
            }
        }
    }
}
