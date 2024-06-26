﻿using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface IOpponentMoveService
    {
        (int X, int Y) GenerateMove(GameState gameState);
    }
}
