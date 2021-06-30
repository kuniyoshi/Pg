#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Pg.Puzzle.Response
{
    public class NewSlidingGems
    {
        public NewSlidingGems(IEnumerable<Coordinate> newGemsCoordinates)
        {
            NewGemsCoordinates = newGemsCoordinates.ToArray();
        }

        IEnumerable<Coordinate> NewGemsCoordinates { get; }
    }
}
