#nullable enable
namespace Pg.Data
{
    public readonly struct Score
    {
        public Score(PointValue pointValue)
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
