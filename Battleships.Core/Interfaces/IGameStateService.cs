using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface IGameStateService
    {
        GameState GetGameState();
        void SaveGameState(GameState gameState);
        ShotResult ProcessShot(int x, int y, bool isPlayer);
        public bool CheckWinCondition(bool testMode = false);
        public void ClearGameState();
    }
}
