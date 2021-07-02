#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Pg.Puzzle.Response
{
    public class NewSlidingGems
    {
        IEnumerable<Coordinate> NewGemsCoordinates { get; }

        public NewSlidingGems(IEnumerable<Coordinate> newGemsCoordinates)
        {
            NewGemsCoordinates = newGemsCoordinates.ToArray();
        }
    }
}
