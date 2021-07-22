#nullable enable
namespace Pg.SceneData.ResultItem
{
    public readonly struct GameResult
    {
        public static GameResult Failure { get; } = new GameResult(Domain.Failure);

        public static GameResult Succeed { get; } = new GameResult(Domain.Succeed);

        Domain Value { get; }

        GameResult(Domain value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is GameResult other)
            {
                return other.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        enum Domain
        {
            Succeed,
            Failure,
        }
    }
}
