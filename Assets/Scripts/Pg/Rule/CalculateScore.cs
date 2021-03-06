#nullable enable
using System.Linq;
using Pg.Data.Response;

namespace Pg.Rule
{
    public static class CalculateScore
    {
        public static AcquisitionScore StepCalculate(VanishingClusters vanishingClusters,
                                                     ChainingCount chainingCount,
                                                     PassedTurn passedTurn)
        {
            var grandTotal = 0;

            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var cluster in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    grandTotal = grandTotal
                                 + CalculateImpl(
                                     new ClusterSize(cluster.Count()),
                                     chainingCount,
                                     passedTurn
                                 );
                }
            }

            return new AcquisitionScore(grandTotal);
        }

        static int CalculateImpl(ClusterSize clusterSize,
                                 ChainingCount chainingCount,
                                 PassedTurn passedTurn)
        {
            var baseValue = 10 * clusterSize.Value + 10 * clusterSize.Value * chainingCount.Value;

            return (int) (passedTurn.GetCoef() * baseValue);
        }
    }
}
