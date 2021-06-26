#nullable enable
namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public SimulationStepData(VanishingClusters vanishingClusters)
        {
            VanishingClusters = vanishingClusters;
        }

        public VanishingClusters VanishingClusters { get; }
    }
}
