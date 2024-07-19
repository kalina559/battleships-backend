using Battleships.Core.Enums;

namespace Battleships.Common.GameClasses
{
    public class GameState
    {
        public List<Ship> UserShips { get; set; } = [];
        public List<Ship> OpponentShips { get; set; } = [];
        public List<Shot> PlayerShots { get; set; } = [];
        public List<Shot> OpponentShots { get; set; } = [];
        public AiType? PlayerAiType { get; set; } = null;
        public AiType OpponentAiType { get; set; }
        public bool ShipsCanTouch { get; set; }
    }
}
