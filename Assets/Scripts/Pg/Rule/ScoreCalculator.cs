#nullable enable
using System.Linq;
using Pg.Data;

namespace Pg.Rule
{
    public class ScoreCalculator
    {
        int _lastChained;

        public void Clear()
        {
            _lastChained = 0;
        }

        public Score StepCalculate(VanishingClusters vanishingClusters)
        {
            var grandTotal = PointValue.Zero;

            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var cluster in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    grandTotal = grandTotal.Add(PointValue.CreateVanished(cluster.Count(), _lastChained));
                }
            }

            _lastChained++;

            return new Score(grandTotal);
        }
    }
}
