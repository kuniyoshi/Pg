#nullable enable
namespace Pg.Data.Internal.Score
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
