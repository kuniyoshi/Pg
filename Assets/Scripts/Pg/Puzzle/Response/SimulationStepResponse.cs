#nullable enable
using Pg.Data.Response;
using Pg.Data.Simulation;

namespace Pg.Puzzle.Response
{
    public class SimulationStepResponse
    {
        public AcquisitionScore AcquisitionScore { get; }

        SimulationStepData SimulationStepData { get; }

        public SimulationStepResponse(SimulationStepData simulationStepData,
                                      AcquisitionScore acquisitionScore)
        {
            SimulationStepData = simulationStepData;
            AcquisitionScore = acquisitionScore;
        }

        public TileStatus[,] BeginningMap => SimulationStepData.BeginningMap;
        public SlidingGems SlidingGems => SimulationStepData.SlidingGems;
        public VanishingClusters VanishingClusters => SimulationStepData.VanishingClusters;

        public override string ToString()
        {
            return $"{nameof(SimulationStepResponse)}{{"
                   + $"{nameof(SimulationStepData)}: {SimulationStepData}"
                   + $", {nameof(AcquisitionScore)}: {AcquisitionScore}"
                   + "}";
        }
    }
}
