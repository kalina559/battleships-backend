using Battleships.Core.Common;
using Battleships.Core.Enums;

namespace Battleships.Services.Interfaces
{
    public interface IAiTypeService
    {
        public List<AiType> GetAllTypes();
        public void SelectAiType(AiType type);
    }
}
