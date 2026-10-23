#nullable enable
using System.Collections.Generic;
using System.Linq;
using Pg.Puzzle.Request;

namespace Pg.Puzzle
{
    public sealed class HintResult
    {
        public IReadOnlyList<GemOperation> Operations { get; }
        public int Score { get; }

        internal HintResult(IEnumerable<GemOperation> operations, int score)
        {
            Operations = System.Array.AsReadOnly(operations.ToArray());
            Score = score;
        }
    }
}
