#nullable enable
using System.Linq;
using Pg.Puzzle.Internal.Score;
using Pg.Puzzle.Response;

namespace Pg.Puzzle.Internal
{
    internal class ScoreCalculator
    {
        int _lastChained;

        internal void Clear()
        {
            _lastChained = 0;
        }

        internal Response.Score StepCalculate(VanishingClusters vanishingClusters)
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

            return new Response.Score(grandTotal);
        }
    }
}
