#nullable enable
using UnityEngine.Assertions;

namespace Pg.Data
{
    public readonly struct VanishPoint
    {
        public static VanishPoint Zero { get; } = new VanishPoint();

        int Value { get; }
        ChainingCount ChainingCount { get; }

        public VanishPoint(int value, ChainingCount chainingCount)
        {
            Value = value;
            ChainingCount = chainingCount;
        }

        public override bool Equals(object? obj)
        {
            return obj is VanishPoint vanishPoint && Equals(vanishPoint);
        }

        public bool Equals(VanishPoint? other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public VanishPoint Add(VanishPoint other)
        {
            Assert.AreEqual(ChainingCount, other.ChainingCount);
            return new VanishPoint(Value + other.Value, ChainingCount);
        }

        public string GetText()
        {
            return Value.ToString();
        }
    }
}
