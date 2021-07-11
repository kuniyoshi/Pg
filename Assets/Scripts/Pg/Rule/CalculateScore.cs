#nullable enable
using System.Linq;
using Pg.Data;

namespace Pg.Rule
{
    public class CalculateScore
    {
        int _lastChained;

        public void Clear()
        {
            _lastChained = 0;
        }

        public Score StepCalculate(VanishingClusters vanishingClusters)
        {
            var grandTotal = VanishPoint.Zero;

            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var cluster in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    grandTotal = grandTotal.Add(CreateVanished(cluster.Count(), _lastChained));
                }
            }

            _lastChained++;

            return new Score(grandTotal);
        }

        static VanishPoint CreateVanished(int clusterSize, int lastChainCount)
        {
            return new VanishPoint(clusterSize * 10 + lastChainCount * clusterSize * 10);
        }
    }
}
