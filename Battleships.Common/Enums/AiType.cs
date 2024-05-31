using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Core.Enums
{
    public enum AiType
    {
        Random = 0, // completely random (excluding cells that were already shot at)
        RandomWithoutAdjacent = 1,  // random, but excluding (excluding cells that were already shot at and the ones adjacent to already sunk ships)
        Heuristic = 2
    }
}
