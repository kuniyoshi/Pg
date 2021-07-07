#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pg.App.Util;
using Pg.Etc.Puzzle;
using Pg.Puzzle;
using Pg.Puzzle.Response;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    public class Coordinates
        : MonoBehaviour
    {
        const int Size = 30;

        [SerializeField]
        GameObject? HexagonPrefab;

        [SerializeField]
        RectTransform? Canvas;

        [SerializeField]
        GraphicRaycaster? GraphicRaycaster;

        Tile[,]? _tiles;
        static Vector2Int DeltaTopLeft { get; } = new Vector2Int(x: -3, y: -2);

        static Vector2 TopLeftOffset { get; } = new Vector2(
            2 * Size * DeltaTopLeft.x * 3f / 4f,
            -Mathf.Sqrt(f: 3f) * Size * DeltaTopLeft.y
        );

        void Awake()
        {
            Assert.IsNotNull(HexagonPrefab, "HexagonPrefab != null");
            Assert.IsNotNull(Canvas, "Canvas != null");
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
                    var hexagon = Instantiate(HexagonPrefab!, Canvas);
                    hexagon.name = $"{HexagonPrefab!.name}({colIndex}, {rowIndex})";
                    var tile = hexagon.GetComponentStrictly<Tile>();
                    tile.Initialize(colIndex, rowIndex, position);

                    _tiles[colIndex, rowIndex] = tile;
                }
            }
        }

        public async Task ApplySlides(SlidingGems slidingGems)
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
                        await _tiles![coordinateFrom.Column, coordinateFrom.Row].Slide();
                        await _tiles![coordinateTo.Column, coordinateTo.Row].Popup(slidingGem.GemColorType);
                        break;

                    case SlidingGems.EventType.NewGem:
                        newGemEnumerator.MoveNext();
                        var newGem = newGemEnumerator.Current;
                        await _tiles![newGem.Coordinate.Column, newGem.Coordinate.Row].NewGem(newGem.GemColorType);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            slidingEnumerator.Dispose();
            newGemEnumerator.Dispose();
        }

        public void ApplyTiles(TileStatus[,] tiles)
        {
            Assert.AreEqual(_tiles!.GetLength(dimension: 0), tiles.GetLength(dimension: 0));
            Assert.AreEqual(_tiles!.GetLength(dimension: 1), tiles.GetLength(dimension: 1));

            for (var rowIndex = 0; rowIndex < _tiles.GetLength(dimension: 1); ++rowIndex)
            {
                for (var colIndex = 0; colIndex < _tiles.GetLength(dimension: 0); ++colIndex)
                {
                    _tiles[colIndex, rowIndex].UpdateStatus(tiles[colIndex, rowIndex]);
                }
            }
        }

        public async Task ApplyVanishings(VanishingClusters vanishingClusters)
        {
            foreach (var gemColorType in vanishingClusters.NewGemColorTypes)
            {
                foreach (var coordinates in vanishingClusters.GetVanishingCoordinatesOf(gemColorType))
                {
                    foreach (var coordinate in coordinates)
                    {
                        await _tiles![coordinate.Column, coordinate.Row].Vanish();
                    }
                }
            }
        }

        public void ClearSelections()
        {
            foreach (var tile in _tiles!)
            {
                tile.ClearSelection();
            }
        }

        public Tile? InterSectWith(Vector2 screenPoint)
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

        public Task SetTileEvents(UserPlayer userPlayer)
        {
            foreach (var tile in _tiles!)
            {
                tile.SetEvents(userPlayer);
            }

            return Task.CompletedTask;
        }
    }
}
