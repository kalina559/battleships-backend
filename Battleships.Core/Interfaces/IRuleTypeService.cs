using Battleships.Common.GameClasses;

namespace Battleships.Services.Interfaces
{
    public interface IRuleTypeService
    {
        public void SelectRuleType(bool shipsCanTouch);
    }
}
