using Battleships.Core.Enums;
using System.Collections.Generic;

namespace Battleships.Common.GameClasses
{
    public class GameState
    {
        public List<Ship> UserShips { get; set; } = [];
        public List<Ship> OpponentShips { get; set; } = [];
        public List<Shot> PlayerShots { get; set; } = [];
        public List<Shot> OpponentShots { get; set; } = [];
        public AiType AiType { get; set; }
    }
}
