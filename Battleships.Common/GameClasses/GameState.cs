using Battleships.Core.Enums;

namespace Battleships.Common.GameClasses
{
    public class GameState
    {
        public PlayerInfo Human { get; set; } = new();
        public PlayerInfo Bot { get; set; } = new();
        public bool ShipsCanTouch { get; set; }
    }

    public class PlayerInfo
    {
        public List<Ship> Ships { get; set; } = [];
        public List<Shot> Shots { get; set; } = [];
        public AiType? AiType { get; set; } = null;
    }
}
