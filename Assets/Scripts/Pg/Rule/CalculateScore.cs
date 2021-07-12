#nullable enable
using System.Linq;
using Pg.Data;

namespace Pg.Rule
{
    public static class CalculateScore
    {
        public static Score StepCalculate(VanishingClusters vanishingClusters, ChainingCount chainingCount)
        {
            var grandTotal = VanishPoint.Zero;

            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var cluster in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    grandTotal = grandTotal.Add(CreateVanished(cluster.Count(), chainingCount));
                }
            }

            return new Score(grandTotal);
        }

        static VanishPoint CreateVanished(int clusterSize, ChainingCount chainingCount)
        {
            return new VanishPoint(clusterSize * 10, chainingCount);
        }
    }
}
