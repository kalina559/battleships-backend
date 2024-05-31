using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Common.GameClasses
{
    public class Ship
    {
        public int Size { get; set; }
        public List<Coordinate> Coordinates { get; set; } = [];
        public bool IsSunk { get; set; } = false;
    }
}
