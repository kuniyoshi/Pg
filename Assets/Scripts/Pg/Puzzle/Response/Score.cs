#nullable enable
using Pg.Puzzle.Internal.Score;

namespace Pg.Puzzle.Response
{
    public readonly struct Score
    {
        internal Score(PointValue pointValue)
        {
            PointValue = pointValue;
        }

        PointValue PointValue { get; }
        public static Score Zero { get; } = new Score(PointValue.Zero);

        public string GetText()
        {
            return PointValue.GetText();
        }

        public override string ToString()
        {
            return GetText();
        }

        public Score Add(Score other)
        {
            return new Score(PointValue.Add(other.PointValue));
        }
    }
}
