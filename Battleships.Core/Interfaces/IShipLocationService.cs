﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Core.Common;

namespace Battleships.Services.Interfaces
{
    public interface IShipLocationService
    {
        List<Ship> GenerateOpponentShips();
    }
}