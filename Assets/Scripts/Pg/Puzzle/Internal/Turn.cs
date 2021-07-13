#nullable enable
using Pg.Data;

namespace Pg.Puzzle.Internal
{
    public class Turn
    {
        public PassedTurn PassedTurn { get; private set; }

        public void Increment()
        {
            PassedTurn = PassedTurn.Increment();
        }
    }
}
