using Battleships.Core.Enums;

namespace Battleships.Common.GameClasses
{
    public class GameState : IDisposable
    {
        public List<Ship> UserShips { get; set; } = [];
        public List<Ship> OpponentShips { get; set; } = [];
        public List<Shot> PlayerShots { get; set; } = [];
        public List<Shot> OpponentShots { get; set; } = [];
        public AiType? PlayerAiType { get; set; } = null;
        public AiType OpponentAiType { get; set; }
        public bool ShipsCanTouch { get; set; }

        public GameState Clone()
        {
            return new GameState
            {
                OpponentAiType = this.OpponentAiType,
                PlayerAiType = this.PlayerAiType,
                ShipsCanTouch = this.ShipsCanTouch,
                OpponentShips = this.OpponentShips,
                OpponentShots = this.OpponentShots,
                PlayerShots = this.PlayerShots,
                UserShips = this.UserShips
                
            };
        }

        public void Dispose()
        {
            // Dispose of any resources if necessary
        }
    }
}
