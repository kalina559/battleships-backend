using System.Collections.Generic;

namespace Battleships.Core.Common
{
    public class GameState
    {
        public List<Ship> UserShips { get; set; } = new List<Ship>();
        public List<Ship> OpponentShips { get; set; } = new List<Ship>();
        public List<Shot> PlayerShots { get; set; } = new List<Shot>();
        public List<Shot> OpponentShots { get; set; } = new List<Shot>();
    }
}
