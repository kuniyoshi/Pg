#nullable enable
using System;
using System.Linq;
using Pg.Data.Simulation;

namespace Pg.Puzzle.Internal
{
    internal class GemGenerator
    {
        int Max { get; }
        int Min { get; }
        Random Random { get; }

        internal GemGenerator()
        {
            Random = new Random();
            Min = GemColorType.Values.Min(value => value.Id);
            Max = GemColorType.Values.Max(value => value.Id);
        }

        internal GemColorType Next()
        {
            const int inclusiveToExclusive = 1;
            return GemColorType.Convert(Random.Next(Min, Max + inclusiveToExclusive));
        }
    }
}
