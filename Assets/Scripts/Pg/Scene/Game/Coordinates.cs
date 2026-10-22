#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Pg.App.Util;
using Pg.Data.Response;
using Pg.Data.Simulation;
using Pg.Etc.Puzzle;
using Pg.Puzzle.Request;
using Pg.Puzzle.Response;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    internal class Coordinates
        : MonoBehaviour
    {
        const int Size = 30;

        [SerializeField]
        GameObject HexagonPrefab = null!;

        [SerializeField]
        GameObject GemPrefab = null!;

        [SerializeField]
        RectTransform TileCanvas = null!;

        [SerializeField]
        RectTransform GemCanvas = null!;

        Tile[,]? _tiles;
        Dictionary<Coordinate, Gem>? _gems;

        GemFactory? _gemFactory;
        UserPlayer? _userPlayer;
        static Vector2Int DeltaTopLeft { get; } = new Vector2Int(x: -3, y: -2);

        static Vector2 TopLeftOffset { get; } = new Vector2(
            2 * Size * DeltaTopLeft.x * 3f / 4f,
            -Mathf.Sqrt(f: 3f) * Size * DeltaTopLeft.y
        );

        void Awake()
        {
            Assert.IsNotNull(HexagonPrefab, "HexagonPrefab != null");
            Assert.IsNotNull(GemPrefab, "GemPrefab != null");
            Assert.IsNotNull(TileCanvas, "TileCanvas != null");
            Assert.IsNotNull(GemCanvas, "GemCanvas != null");
        }

        void CreateTiles()
        {
            var positions = new Vector2[TileSize.ColSize, TileSize.RowSize];
            var offsetY = Mathf.Sqrt(f: 3f) * Size * 0.5f;
            var intervalX = 2 * Size * 3f / 4f;
            var intervalY = Mathf.Sqrt(f: 3f) * Size;

            for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
            {
                for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
                {
                    var offset = colIndex % 2 == 0 ? 0f : offsetY;
                    var position = new Vector2(colIndex * intervalX, -intervalY * rowIndex + offset);
                    positions[colIndex, rowIndex] = TopLeftOffset + position;
                }
            }

            _tiles = new Tile[TileSize.ColSize, TileSize.RowSize];

            for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
            {
                for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
                {
                    var position = positions[colIndex, rowIndex];
                    var hexagon = Instantiate(HexagonPrefab, TileCanvas);
                    hexagon.name = $"{HexagonPrefab.name}({colIndex}, {rowIndex})";
                    var tile = hexagon.GetComponentStrictly<Tile>();
                    tile.Initialize(colIndex, rowIndex, position);

                    _tiles[colIndex, rowIndex] = tile;
                }
            }

            _gems = new Dictionary<Coordinate, Gem>();

            _gemFactory = new GemFactory(GemPrefab);
        }

        Gem CreateGem(Coordinate coordinate, GemColorType color)
        {
            var gem = _gemFactory!.InstantiateNewGem(GemCanvas);
            gem.Initialize(_tiles![coordinate.Column, coordinate.Row], color, _userPlayer!);
            _gems!.Add(coordinate, gem);
            return gem;
        }

        internal async Task ApplySlides(SlidingGems slidingGems)
        {
            using var slidingEnumerator = slidingGems.Items.GetEnumerator();
            using var newGemEnumerator = slidingGems.NewGems.GetEnumerator();

            foreach (var eventType in slidingGems.EventTypes)
            {
                switch (eventType)
                {
                    case SlidingGems.EventType.Take:
                        if (!slidingEnumerator.MoveNext()) throw new InvalidOperationException("Missing sliding gem event");
                        var slide = slidingEnumerator.Current;
                        Assert.IsTrue(_gems!.ContainsKey(slide.From));
                        Assert.IsFalse(_gems!.ContainsKey(slide.To));
                        var gem = _gems[slide.From];
                        _gems.Remove(slide.From);
                        _gems.Add(slide.To, gem);
                        await gem.SlideTo(_tiles![slide.To.Column, slide.To.Row]);
                        break;
                    case SlidingGems.EventType.NewGem:
                        if (!newGemEnumerator.MoveNext()) throw new InvalidOperationException("Missing new gem event");
                        var newGem = newGemEnumerator.Current;
                        Assert.IsFalse(_gems!.ContainsKey(newGem.Coordinate));
                        await CreateGem(newGem.Coordinate, newGem.GemColorType).Popup();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal void Initialize(TileStatus[,] tiles, UserPlayer userPlayer)
        {
            _userPlayer = userPlayer;
            CreateTiles();
            Assert.AreEqual(_tiles!.GetLength(0), tiles.GetLength(0));
            Assert.AreEqual(_tiles!.GetLength(1), tiles.GetLength(1));
            for (var row = 0; row < tiles.GetLength(1); ++row)
            {
                for (var col = 0; col < tiles.GetLength(0); ++col)
                {
                    var status = tiles[col, row];
                    _tiles[col, row].SetClosed(status.TileStatusType == TileStatusType.Closed);
                    if (status.GemColorType != null)
                    {
                        CreateGem(new Coordinate(col, row), status.GemColorType);
                    }
                }
            }
        }

        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        internal void AssertMatches(TileStatus[,] tiles)
        {
            var gemCount = 0;
            for (var row = 0; row < tiles.GetLength(1); ++row)
            {
                for (var col = 0; col < tiles.GetLength(0); ++col)
                {
                    var coordinate = new Coordinate(col, row);
                    var color = tiles[col, row].GemColorType;
                    if (color == null)
                    {
                        Assert.IsFalse(_gems!.ContainsKey(coordinate));
                        continue;
                    }
                    ++gemCount;
                    Assert.IsTrue(_gems!.ContainsKey(coordinate));
                    Assert.AreEqual(coordinate, _gems![coordinate].Coordinate);
                    Assert.AreEqual(color, _gems[coordinate].Color);
                }
            }
            Assert.AreEqual(gemCount, _gems!.Count);
        }

        class GemFactory
        {
            GameObject Prefab { get; }
            int _id;

            internal GemFactory(GameObject prefab)
            {
                Prefab = prefab;
            }

            internal Gem InstantiateNewGem(Transform parentTransform)
            {
                var instance = Instantiate(Prefab, parentTransform);
                instance.name = $"{Prefab.name}({_id++})";
                return instance.GetComponentStrictly<Gem>();
            }
        }

        internal async Task ApplyVanishings(VanishingClusters vanishingClusters)
        {
            var vanishings = new List<Gem>();
            foreach (var coordinate in vanishingClusters.WholeCoordinates.Distinct())
            {
                Assert.IsTrue(_gems!.ContainsKey(coordinate));
                vanishings.Add(_gems![coordinate]);
                _gems.Remove(coordinate);
            }
            await UniTask.WhenAll(vanishings.Select(gem => gem.Vanish()));
        }

        internal void ClearSelections()
        {
            foreach (var gem in _gems!.Values) gem.QuitWorking();
        }

        internal void Swap(GemOperation operation)
        {
            var (a, b) = (operation.A, operation.B);
            Assert.IsTrue(_gems!.ContainsKey(a));
            Assert.IsTrue(_gems!.ContainsKey(b));
            (_gems[a], _gems[b]) = (_gems[b], _gems[a]);
        }
    }
}
