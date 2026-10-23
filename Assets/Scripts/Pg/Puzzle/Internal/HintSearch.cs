#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Pg.Data.Response;
using Pg.Data.Simulation;
using Pg.Puzzle.Request;
using Pg.Rule;

namespace Pg.Puzzle.Internal
{
    // Search a single drag. Resolve a separate copy only when evaluating a release.
    internal sealed class HintSearch
    {
        readonly TileStatus[,] _tiles;
        readonly GemGenerator _supply;
        readonly PassedTurn _turn;

        internal HintSearch(TileStatus[,] tiles, GemGenerator supply, PassedTurn turn)
        {
            _tiles = tiles;
            _supply = supply;
            _turn = turn;
        }

        internal HintResult? Find(int beamWidth = 24, int maxSwaps = 8)
        {
            if (beamWidth < 1 || maxSwaps < 1) throw new ArgumentOutOfRangeException();
            var beam = new List<Node>();
            for (var x = 0; x < _tiles.GetLength(0); x++)
                for (var y = 0; y < _tiles.GetLength(1); y++)
                    if (_tiles[x, y].TileStatusType == TileStatusType.Contain)
                        beam.Add(new Node(new Map(_tiles), new Coordinate(x, y), Array.Empty<GemOperation>(), 0));

            Node? best = null;
            for (var depth = 0; depth < maxSwaps; depth++)
            {
                var candidates = new List<Node>();
                var seen = new HashSet<string>();
                foreach (var node in beam)
                {
                    for (var direction = 0; direction < DirectionService.NeighborSize; direction++)
                    {
                        var next = DirectionService.GetNeighborOf(node.Cursor, direction);
                        if (!node.Map.IsCoordinateInRange(next) ||
                            node.Map.GetTileStatusAt(next).TileStatusType != TileStatusType.Contain) continue;
                        // An immediate reversal adds no new board state.
                        if (node.Operations.Length > 0 && next == node.Operations.Last().A) continue;
                        var map = new Map(node.Map.CurrentTileStatuses);
                        map.Swap(node.Cursor, next);
                        var key = next + ":" + string.Concat(map.CurrentTileStatuses.Cast<TileStatus>().Select(t => t.Sigil));
                        if (!seen.Add(key)) continue;
                        var score = Evaluate(map);
                        if (!score.HasValue) continue;
                        var operations = node.Operations.Concat(new[] { new GemOperation(node.Cursor, next) }).ToArray();
                        var candidate = new Node(map, next, operations, score.Value);
                        candidates.Add(candidate);
                        if (best == null || candidate.Score > best.Score) best = candidate;
                    }
                }
                beam = candidates.OrderByDescending(n => n.Score).Take(beamWidth).ToList();
                if (beam.Count == 0) break;
            }
            return best == null ? null : new HintResult(best.Operations, best.Score);
        }

        int? Evaluate(Map candidate)
        {
            var map = new Map(candidate.CurrentTileStatuses);
            var worker = new Worker(_supply.Clone());
            var score = 0;
            var chain = 0;
            foreach (var step in worker.ProcessTurn(map, Array.Empty<GemOperation>()))
            {
                // Repeating supplies can produce an endless cascade. Never hang hint search.
                if (chain >= 128) return null;
                score += CalculateScore.StepCalculate(step.VanishingClusters, new ChainingCount(chain++), _turn).Value;
            }
            return score;
        }

        sealed class Node
        {
            internal readonly Map Map;
            internal readonly Coordinate Cursor;
            internal readonly GemOperation[] Operations;
            internal readonly int Score;
            internal Node(Map map, Coordinate cursor, GemOperation[] operations, int score)
            {
                Map = map;
                Cursor = cursor;
                Operations = operations;
                Score = score;
            }
        }
    }
}
