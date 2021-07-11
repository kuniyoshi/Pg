#nullable enable
namespace Pg.Data
{
    public readonly struct Score
    {
        public Score(VanishPoint vanishPoint)
        {
            VanishPoint = vanishPoint;
        }

        VanishPoint VanishPoint { get; }
        public static Score Zero { get; } = new Score(VanishPoint.Zero);

        public string GetText()
        {
            return VanishPoint.GetText();
        }

        public override string ToString()
        {
            return GetText();
        }

        public Score Add(Score other)
        {
            return new Score(VanishPoint.Add(other.VanishPoint));
        }
    }
}
