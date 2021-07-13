#nullable enable
using UnityEngine.Assertions;

namespace Pg.Data
{
    public readonly struct VanishPoint
    {
        public static VanishPoint Zero { get; } = new VanishPoint();

        ClusterSize ClusterSize { get; }
        ChainingCount ChainingCount { get; }

        public VanishPoint(ClusterSize clusterSize, ChainingCount chainingCount)
        {
            ClusterSize = clusterSize;
            ChainingCount = chainingCount;
        }

        public override bool Equals(object? obj)
        {
            return obj is VanishPoint vanishPoint && Equals(vanishPoint);
        }

        public bool Equals(VanishPoint? other)
        {
            return ClusterSize == other?.ClusterSize
                   && ChainingCount == other?.ChainingCount;
        }

        public override int GetHashCode()
        {
            var hashCodeA = ClusterSize.GetHashCode();
            var hashCodeB = ChainingCount.GetHashCode();

            return new {hashCodeA, hashCodeB}.GetHashCode();
        }

        public VanishPoint Add(VanishPoint other)
        {
            Assert.AreEqual(ChainingCount, other.ChainingCount);
            return new VanishPoint(ClusterSize, ChainingCount);
        }

        public string GetText()
        {
            return ClusterSize.ToString();
        }
    }
}
