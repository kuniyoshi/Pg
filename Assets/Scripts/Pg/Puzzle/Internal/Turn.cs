#nullable enable
using Pg.Data.Response;

namespace Pg.Puzzle.Internal
{
    internal class Turn
    {
        internal PassedTurn PassedTurn { get; private set; }

        internal void Increment()
        {
            PassedTurn = PassedTurn.Increment();
        }
    }
}
