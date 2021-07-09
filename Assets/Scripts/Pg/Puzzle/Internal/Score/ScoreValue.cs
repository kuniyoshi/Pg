#nullable enable
namespace Pg.Puzzle.Internal.Score
{
    internal readonly struct ScoreValue
    {
        PointValue PointValue { get; }

        internal ScoreValue(PointValue pointValue)
        {
            PointValue = pointValue;
        }
    }
}
