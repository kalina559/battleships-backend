using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface IShipLocationService
    {
        List<Ship> GenerateOpponentShips();
    }
}
