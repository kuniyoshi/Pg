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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Coordinates
        : MonoBehaviour
    {
        const int Size = 30;

        [SerializeField]
        GameObject? HexagonPrefab;

        [SerializeField]
        GameObject? GemPrefab;

        [SerializeField]
        RectTransform? TileCanvas;

        [SerializeField]
        RectTransform? GemCanvas;

        [SerializeField]
        GraphicRaycaster? GraphicRaycaster;

        Tile[,]? _tiles;
        Dictionary<Coordinate, Gem2>? _gems;

        GemFactory? _gemFactory;
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
            Assert.IsNotNull(GraphicRaycaster, "GraphicRaycaster != null");
        }

        void Start()
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
                    var hexagon = Instantiate(HexagonPrefab!, TileCanvas);
                    hexagon.name = $"{HexagonPrefab!.name}({colIndex}, {rowIndex})";
                    var tile = hexagon.GetComponentStrictly<Tile>();
                    tile.Initialize(colIndex, rowIndex, position);

                    _tiles[colIndex, rowIndex] = tile;
                }
            }

            _gems = new Dictionary<Coordinate, Gem2>();

            _gemFactory = new GemFactory(GemPrefab!);
        }

        internal async Task ApplySlides(SlidingGems slidingGems)
        {
            var slidingEnumerator = slidingGems.Items.GetEnumerator();
            var newGemEnumerator = slidingGems.NewGems.GetEnumerator();


            foreach (var slidingGemsEventType in slidingGems.EventTypes)
            {
                switch (slidingGemsEventType)
                {
                    case SlidingGems.EventType.Take:
                        slidingEnumerator.MoveNext();
                        var slidingGem = slidingEnumerator.Current;
                        var (coordinateFrom, coordinateTo) = (slidingGem.From, slidingGem.To);
                        Assert.IsTrue(_gems!.ContainsKey(coordinateFrom), "_gems!.ContainsKey(coordinateFrom)");
                        await _gems![coordinateFrom].SlideTo(_tiles![coordinateTo.Column, coordinateTo.Row]);
                        Slide(coordinateFrom, coordinateTo);
                        break;

                    case SlidingGems.EventType.NewGem:
                        newGemEnumerator.MoveNext();
                        var newGem = newGemEnumerator.Current;
                        Assert.IsTrue(!_gems!.ContainsKey(newGem.Coordinate), "!_gems!.ContainsKey(newGem.Coordinate)");
                        var gem = _gemFactory!.InstantiateNewGem(GemCanvas!);
                        gem.Initialize(_tiles![newGem.Coordinate.Column, newGem.Coordinate.Row], newGem.GemColorType);
                            // await _tiles![newGem.Coordinate.Column, newGem.Coordinate.Row].NewGem(newGem.GemColorType);
                        await gem.Popup();
                        _gems!.Add(newGem.Coordinate, gem);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            slidingEnumerator.Dispose();
            newGemEnumerator.Dispose();
        }

        void Slide(Coordinate from, Coordinate to)
        {
            Assert.IsTrue(_gems!.ContainsKey(from), "_gems!.ContainsKey(from)");
            Assert.IsTrue(!_gems!.ContainsKey(to), "!_gems!.ContainsKey(to)");
            _gems!.Add(to, _gems![from]);
            _gems!.Remove(from);
        }

        internal void ApplyTiles(TileStatus[,] tiles, UserPlayer userPlayer)
        {
            Assert.AreEqual(_tiles!.GetLength(dimension: 0), tiles.GetLength(dimension: 0));
            Assert.AreEqual(_tiles!.GetLength(dimension: 1), tiles.GetLength(dimension: 1));

            for (var rowIndex = 0; rowIndex < _tiles.GetLength(dimension: 1); ++rowIndex)
            {
                for (var colIndex = 0; colIndex < _tiles.GetLength(dimension: 0); ++colIndex)
                {
                    _tiles[colIndex, rowIndex].UpdateStatus2(tiles[colIndex, rowIndex].TileStatusType);

                    if (tiles[colIndex, rowIndex].GemColorType != null)
                    {
                        if (!_gems!.ContainsKey(new Coordinate(colIndex, rowIndex)))
                        {
                            var gem = _gemFactory!.InstantiateNewGem(GemCanvas!);
                            gem.Initialize(_tiles[colIndex, rowIndex], tiles[colIndex, rowIndex].GemColorType!);
                            gem.Bind(userPlayer);
                            _gems!.Add(new Coordinate(colIndex, rowIndex), gem);
                        }
                    }
                    // _tiles[colIndex, rowIndex].UpdateStatus(tiles[colIndex, rowIndex]);
                }
            }
        }

        class GemFactory
        {
            GameObject Prefab { get; }

            int _id;

            internal GemFactory(GameObject prefab)
            {
                Prefab = prefab;
            }

            internal Gem2 InstantiateNewGem(Transform parentTransform)
            {
                var newGameObject = Instantiate(Prefab, parentTransform);
                newGameObject.name = $"{Prefab.name}({_id++})";

                return newGameObject.GetComponentStrictly<Gem2>();
            }
        }

        internal async Task ApplyVanishings(VanishingClusters vanishingClusters)
        {
            var vanishings = new List<Gem2>();

            foreach (var coordinate in vanishingClusters.WholeCoordinates)
            {
                Assert.IsTrue(_gems!.ContainsKey(coordinate), "_gems!.ContainsKey(coordinate)");
                vanishings.Add(_gems![coordinate]);
                _gems!.Remove(coordinate);
            }

            // Debug.Log($"### Vanishings: {string.Join(", ", vanishingClusters.WholeCoordinates.Select(c => c))}");

            await UniTask.WhenAll(Enumerable.Select(vanishings, gem => gem.Vanish()));
        }

        internal void ClearSelections()
        {
            // foreach (var tile in _tiles!)
            // {
            //     tile.Gem!.QuitWorking();
            // }

            foreach (var gem in _gems!.Values)
            {
                gem.QuitWorking();
            }
        }

        internal Tile? InterSectWith(Vector2 screenPoint)
        {
            var raycastResults = new List<RaycastResult>();
            var pointerEventData = new PointerEventData(EventSystem.current)
            {
                delta = screenPoint,
            };
            GraphicRaycaster!.Raycast(pointerEventData, raycastResults);

            if (!raycastResults.Any())
            {
                return null;
            }

            var raycastResult = raycastResults.First();
            Assert.IsTrue(raycastResult.isValid, "raycastResult.isValid");

            foreach (var tile in _tiles!)
            {
                if (raycastResult.gameObject == tile.gameObject)
                {
                    return tile;
                }
            }

            return null;
        }

        internal Task SetTileEvents(UserPlayer userPlayer)
        {
            foreach (var tile in _tiles!)
            {
                tile.SetEvents(userPlayer);
            }

            return Task.CompletedTask;
        }

        public void Swap(GemOperation gemOperation)
        {
            var (a, b) = (gemOperation.A, gemOperation.B);
            Assert.IsTrue(_gems!.ContainsKey(a), "_gems!.ContainsKey(a)");
            Assert.IsTrue(_gems!.ContainsKey(b), "_gems!.ContainsKey(b)");
            (_gems![a], _gems![b]) = (_gems![b], _gems![a]);
        }
    }
}
