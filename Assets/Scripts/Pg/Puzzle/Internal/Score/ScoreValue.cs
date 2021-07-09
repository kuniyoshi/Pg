#nullable enable
namespace Pg.Puzzle.Internal.Score
{
    public readonly struct ScoreValue
    {
        PointValue PointValue { get; }

        public ScoreValue(PointValue pointValue)
        {
            PointValue = pointValue;
        }
    }
}
