#nullable enable
namespace Pg.Puzzle.Response
{
    public class SimulationStepData
    {
        public SimulationStepData(VanishingClusters vanishingClusters,
                                  SlidingGems slidingGems,
                                  NewSlidingGems newSlidingGems)
        {
            SlidingGems = slidingGems;
            NewSlidingGems = newSlidingGems;
            VanishingClusters = vanishingClusters;
        }

        public SlidingGems SlidingGems { get; }

        public NewSlidingGems NewSlidingGems { get; }

        public VanishingClusters VanishingClusters { get; }

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
