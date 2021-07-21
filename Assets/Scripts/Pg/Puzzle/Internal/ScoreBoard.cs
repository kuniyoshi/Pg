#nullable enable
using Pg.Data.Response;

namespace Pg.Puzzle.Internal
{
    internal class ScoreBoard
    {
        Score _currentScore = Score.Zero;
        internal Score Current => _currentScore;

        internal void Add(AcquisitionScore acquisitionScore)
        {
            _currentScore = _currentScore.Add(acquisitionScore);
        }
    }
}
