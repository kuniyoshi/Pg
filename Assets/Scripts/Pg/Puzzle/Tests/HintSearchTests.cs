#nullable enable
using System;
using System.Linq;
using NUnit.Framework;
using Pg.Data.Request;
using Pg.Data.Response;
using Pg.Data.Simulation;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Internal;
using Pg.Puzzle.Request;

namespace Pg.Puzzle.Tests
{
    public class HintSearchTests
    {
        static TileStatus[,] Board()
        {
            var tiles = new TileStatus[TileSize.ColSize, TileSize.RowSize];
            var colors = GemColorType.Values.Where(c => c != GemColorType.Rainbow).ToArray();
            var random = new Random(123);
            for (var x = 0; x < tiles.GetLength(0); x++)
                for (var y = 0; y < tiles.GetLength(1); y++)
                    tiles[x, y] = new TileStatus(TileStatusType.Contain, colors[random.Next(colors.Length)]);
            return tiles;
        }

        [Test]
        public void SupplyClonePreservesCursorWithoutConsumingOriginal()
        {
            var supply = new GemGenerator(new[] { GemColorType.Red, GemColorType.Blue }, 2);
            supply.Next();
            var clone = supply.Clone();
            Assert.AreEqual(GemColorType.Blue, clone.Next());
            clone.Next();
            Assert.AreEqual(GemColorType.Blue, supply.Next());
        }

        [Test]
        public void HintMatchesActualTurnAndDoesNotChangeLiveState()
        {
            var data = new Data(Board());
            var random = new Random(456);
            var colors = GemColorType.Values.ToArray();
            var supply = Enumerable.Range(0, GameController.CalculateRequiredGemCount(data.Stage))
                .Select(_ => colors[random.Next(colors.Length)]).ToArray();
            var live = new GameController(data, supply);
            var control = new GameController(data, supply);
            // Advance supply and turn first, so the snapshot must preserve both.
            var first = new[] { new GemOperation(new Coordinate(0, 0), new Coordinate(0, 1)) };
            live.ProcessTurn(first);
            control.ProcessTurn(first);
            var before = (TileStatus[,])live.DebugGetTileStatuses().Clone();
            var hint = live.FindHintAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(hint);
            CollectionAssert.AreEqual(before, live.DebugGetTileStatuses());
            Assert.AreEqual(control.PassedTurn, live.PassedTurn);
            var secondHint = live.FindHintAsync().GetAwaiter().GetResult();
            CollectionAssert.AreEqual(hint!.Operations, secondHint!.Operations);
            for (var i = 0; i < hint.Operations.Count; i++)
            {
                var operation = hint.Operations[i];
                Assert.IsTrue(GameRule.CanSwap(operation.A, operation.B));
                if (i > 0) Assert.AreEqual(hint.Operations[i - 1].B, operation.A);
            }
            var previousScore = control.ProcessTurn(hint.Operations).Score.GetValue();
            var actual = live.ProcessTurn(hint.Operations);
            Assert.AreEqual(previousScore, actual.Score.GetValue());
            Assert.AreEqual(hint.Score, actual.SimulationStepResponses.Sum(s => s.AcquisitionScore.Value));
            CollectionAssert.AreEqual(control.DebugGetTileStatuses(), live.DebugGetTileStatuses());
        }

        [Test]
        public void SingleSwapSearchFindsHighestActualScore()
        {
            var tiles = Board();
            var random = new Random(789);
            var colors = GemColorType.Values.ToArray();
            var gems = Enumerable.Range(0, tiles.Length * 10)
                .Select(_ => colors[random.Next(colors.Length)]).ToArray();
            var search = new HintSearch(tiles, new GemGenerator(gems, 1), new PassedTurn(0));
            var hint = search.Find(beamWidth: 1, maxSwaps: 1);
            var best = 0;
            for (var x = 0; x < tiles.GetLength(0); x++)
                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    var a = new Coordinate(x, y);
                    for (var d = 0; d < DirectionService.NeighborSize; d++)
                    {
                        var b = DirectionService.GetNeighborOf(a, d);
                        if (!new Map(tiles).IsCoordinateInRange(b)) continue;
                        var game = new GameController(new Data(tiles), gems);
                        var score = game.ProcessTurn(new[] { new GemOperation(a, b) }).Score.GetValue();
                        best = Math.Max(best, score);
                    }
                }
            Assert.AreEqual(best, hint!.Score);
        }

        [Test]
        public void ClosedBoardHasNoHint()
        {
            var tiles = Board();
            for (var x = 0; x < tiles.GetLength(0); x++)
                for (var y = 0; y < tiles.GetLength(1); y++)
                    tiles[x, y] = new TileStatus(TileStatusType.Closed, null);
            Assert.IsNull(new HintSearch(tiles,
                new GemGenerator(new[] { GemColorType.Red }, 1), new PassedTurn(0)).Find());
        }

        sealed class Data : IGameData, IStage
        {
            internal Data(TileStatus[,] tiles) { TileStatuses = tiles; }
            public IStage Stage => this;
            public int MaxTurnCount => 10;
            public int TargetScore => 1000000;
            public TileStatus[,] TileStatuses { get; }
        }
    }
}
