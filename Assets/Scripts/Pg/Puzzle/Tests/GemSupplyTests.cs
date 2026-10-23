#nullable enable
using System;
using System.Linq;
using NUnit.Framework;
using Pg.Data.Request;
using Pg.Data.Simulation;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Internal;
using Pg.Puzzle.Request;
using Assert = NUnit.Framework.Assert;

namespace Pg.Puzzle.Tests
{
    public class GemSupplyTests
    {
        [TestCase(7, 6, 10, 420)]
        [TestCase(3, 8, 5, 120)]
        [TestCase(1, 1, 1, 1)]
        public void RequiredCountUsesActualDimensions(int columns, int rows, int turns, int expected)
        {
            Assert.AreEqual(expected, GameController.CalculateRequiredGemCount(new Stage(columns, rows, turns)));
        }

        [Test]
        public void RequiredCountDetectsOverflow()
        {
            Assert.Throws<OverflowException>(() =>
                GameController.CalculateRequiredGemCount(new Stage(2, 1, int.MaxValue)));
        }

        [Test]
        public void SupplyRepeatsEntireInputAndCopiesIt()
        {
            var gems = new[] { GemColorType.Red, GemColorType.Blue, GemColorType.Green };
            var generator = new GemGenerator(gems, 2);
            gems[0] = GemColorType.Yellow;
            var expected = new[] { GemColorType.Red, GemColorType.Blue, GemColorType.Green,
                GemColorType.Red, GemColorType.Blue, GemColorType.Green, GemColorType.Red };
            CollectionAssert.AreEqual(expected, expected.Select(_ => generator.Next()).ToArray());
        }

        [Test]
        public void SupplyRejectsInsufficientAndEmptyBuffers()
        {
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() =>
                new GemGenerator(new[] { GemColorType.Red }, 2));
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() =>
                new GemGenerator(Array.Empty<GemColorType>(), 1));
            Assert.Throws<UnityEngine.Assertions.AssertionException>(() =>
                new GemGenerator(new[] { GemColorType.Red }, 0));
        }

        [Test]
        public void WorkerKeepsCursorAcrossTurnsAndDoesNotConsumeWithoutEmptyTiles()
        {
            var generator = new GemGenerator(new[] { GemColorType.Red, GemColorType.Blue }, 2);
            var worker = new Worker(generator);
            var tiles = new TileStatus[TileSize.ColSize, TileSize.RowSize];
            for (var x = 0; x < TileSize.ColSize; x++)
                for (var y = 0; y < TileSize.RowSize; y++)
                    tiles[x, y] = new TileStatus(TileStatusType.Closed, null);
            var map = new Map(tiles);
            var top = new Coordinate(0, 0);
            worker.ProcessTurn(map, Array.Empty<GemOperation>()).ToArray();
            map.SetEmpty(top);
            worker.ProcessTurn(map, Array.Empty<GemOperation>()).ToArray();
            Assert.AreEqual(GemColorType.Red, map.GetTileStatusAt(top).GemColorType);
            map.SetEmpty(top);
            worker.ProcessTurn(map, Array.Empty<GemOperation>()).ToArray();
            Assert.AreEqual(GemColorType.Blue, map.GetTileStatusAt(top).GemColorType);
            map.SetEmpty(top);
            worker.ProcessTurn(map, Array.Empty<GemOperation>()).ToArray();
            Assert.AreEqual(GemColorType.Red, map.GetTileStatusAt(top).GemColorType);
        }

        class Stage : IStage
        {
            public Stage(int columns, int rows, int turns)
            {
                TileStatuses = new TileStatus[columns, rows];
                MaxTurnCount = turns;
            }
            public int MaxTurnCount { get; }
            public int TargetScore => 100;
            public TileStatus[,] TileStatuses { get; }
        }
    }
}
