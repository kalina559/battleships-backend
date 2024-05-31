using Battleships.Core.Common;
using Battleships.Core.Enums;

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
