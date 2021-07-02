#nullable enable
namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public NewSlidingGems NewSlidingGems { get; }

        public SlidingGems SlidingGems { get; }

        public VanishingClusters VanishingClusters { get; }

        public SimulationStepData(VanishingClusters vanishingClusters,
                                  SlidingGems slidingGems,
                                  NewSlidingGems newSlidingGems)
        {
            SlidingGems = slidingGems;
            NewSlidingGems = newSlidingGems;
            VanishingClusters = vanishingClusters;
        }

        public override string ToString()
        {
            return $"{nameof(SimulationStepData)}{{"
                   + $"{nameof(VanishingClusters)}: {VanishingClusters}"
                   + $", {nameof(SlidingGems)}: {SlidingGems}"
                   + $", {nameof(NewSlidingGems)}: {NewSlidingGems}"
                   + "}";
        }
    }
}
