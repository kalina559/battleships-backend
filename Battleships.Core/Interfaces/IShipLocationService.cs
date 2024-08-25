using Battleships.Common.Enums;
using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface IShipLocationService
    {
        List<Ship> GenerateOpponentShips(List<int>? shipSizes = null, BiasType biasType = BiasType.None);
    }
}
