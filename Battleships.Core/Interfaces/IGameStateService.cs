using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Core.Common;

namespace Battleships.Services.Interfaces
{
    public interface IGameStateService
    {
        GameState GetGameState();
        void SaveGameState(GameState gameState);
        ShotResult ProcessShot(int x, int y, bool isPlayer);
        public bool CheckWinCondition();
        public void ClearGameState();
    }
}
