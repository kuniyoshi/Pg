#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Pg.Puzzle.Response
{
    public class SlidingGems
    {
        public SlidingGems(IEnumerable<SlidingGem> slidingGems)
        {
            Items = slidingGems.ToArray();
        }

        public IEnumerable<SlidingGem> Items { get; }

        public override string ToString()
        {
            return $"{nameof(SlidingGem)}{{"
                   + $"{nameof(Items)}: [{string.Join(", ", Items.Select(item => item.ToString()))}]"
                   + "}";
        }

        public readonly struct SlidingGem
        {
            public SlidingGem(GemColorType gemColorType, Coordinate from, Coordinate to)
            {
                GemColorType = gemColorType;
                From = from;
                To = to;
            }

            public Coordinate From { get; }
            public Coordinate To { get; }
            public GemColorType GemColorType { get; }

            public override string ToString()
            {
                return $"{nameof(SlidingGem)}{{"
                       + $"{nameof(GemColorType)}: {GemColorType}"
                       + $", {nameof(From)}: {From}"
                       + $", {nameof(To)}: {To}"
                       + "}";
            }
        }
    }
}
