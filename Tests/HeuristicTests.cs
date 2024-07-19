using Battleships.Common.GameClasses;
using Battleships.Core.Enums;
using Battleships.Core.Services;
using Battleships.Common.Helpers;

namespace Battleships.UnitTests
{
    public class HeuristicTests
    {
        private readonly GenerateMoveService _opponentMoveService;

        public HeuristicTests()
        {
            _opponentMoveService = new GenerateMoveService();
        }

        [Fact]
        public void RandomVsLocationHeuristic()
        {
            
        }
    }
}