using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Core.Common
{
    public class Ship
    {
        public int Size { get; set; }
        public List<Position> Coordinates { get; set; } = [];
        public bool IsSunk { get; set; } = false;
    }
}
