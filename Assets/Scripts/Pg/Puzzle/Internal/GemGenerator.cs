#nullable enable
using System.Collections.Generic;
using System.Linq;
using Pg.Data.Simulation;
using UnityEngine.Assertions;

namespace Pg.Puzzle.Internal
{
    internal class GemGenerator
    {
        GemColorType[] Gems { get; }
        int _index;

        internal GemGenerator(IReadOnlyList<GemColorType> gems, int requiredCount)
        {
            Assert.IsTrue(requiredCount > 0);
            Assert.IsNotNull(gems);
            Assert.IsTrue(gems.Count >= requiredCount, "Insufficient replenishment gems.");
            Assert.IsTrue(gems.All(gem => gem != null));
            Gems = gems.ToArray();
        }

        GemGenerator(GemColorType[] gems, int index)
        {
            Gems = gems;
            _index = index;
        }

        internal GemGenerator Clone() => new GemGenerator(Gems, _index);

        internal GemColorType Next()
        {
            var gem = Gems[_index];
            _index = (_index + 1) % Gems.Length;
            return gem;
        }
    }
}
