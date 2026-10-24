#nullable enable
using System;

namespace Pg.SceneData.ResultItem
{
    [Serializable]
    public readonly struct GameResult : IEquatable<GameResult>
    {
        public static bool operator ==(GameResult a, GameResult b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(GameResult a, GameResult b)
        {
            return !(a == b);
        }

        GameResult(Domain value)
        {
            Value = value;
        }

        public static GameResult Failure { get; } = new GameResult(Domain.Failure);

        public static GameResult Success { get; } = new GameResult(Domain.Success);

        Domain Value { get; }

        public override bool Equals(object? obj)
        {
            return obj is GameResult other && Equals(other);
        }

        public bool Equals(GameResult other)
        {
            return Value == other.Value;
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
            Success,
            Failure,
        }
    }
}
