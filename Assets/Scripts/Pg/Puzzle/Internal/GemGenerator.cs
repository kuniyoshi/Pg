#nullable enable
using System;

namespace Pg.Puzzle.Internal
{
    internal class GemGenerator
    {
        static (int Min, int Max) GetMinMax()
        {
            var values = Enum.GetValues(typeof(GemColorType));
            var min = (int) ((GemColorType) values.GetValue(index: 0));
            var max = min;

            foreach (var value in values)
            {
                var gemColorType = (GemColorType) value;
                var candidate = (int) gemColorType;
                min = candidate < min ? candidate : min;
                max = candidate > max ? candidate : max;
            }

            return (min, max);
        }

        int Max { get; }
        int Min { get; }
        Random Random { get; }

        internal GemGenerator()
        {
            Random = new Random();
            (Min, Max) = GetMinMax();
        }

        internal GemColorType Next()
        {
            const int inclusiveToExclusive = 1;
            return (GemColorType) Random.Next(Min, Max + inclusiveToExclusive);
        }
    }
}
