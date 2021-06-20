#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pg.App.Util;
using Pg.Etc.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    public class Coordinates
        : MonoBehaviour
    {
        [SerializeField]
        GameObject? HexagonPrefab;

        [SerializeField]
        RectTransform? Canvas;

        Tile[,]? _tiles;
        const int Size = 30;
        static Vector2Int DeltaTopLeft { get; } = new Vector2Int(-3, -2);

        static Vector2 TopLeftOffset { get; } = new Vector2(
            2 * Size * DeltaTopLeft.x * 3f / 4f,
            -Mathf.Sqrt(3f) * Size * DeltaTopLeft.y
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
            var offsetY = Mathf.Sqrt(3f) * Size * 0.5f;
            var intervalX = 2 * Size * 3f / 4f;
            var intervalY = Mathf.Sqrt(3f) * Size;

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

        public void ApplyTiles(TileStatus[,] tiles)
        {
            Assert.AreEqual(_tiles!.GetLength(0), tiles.GetLength(0));
            Assert.AreEqual(_tiles!.GetLength(1), tiles.GetLength(1));

            for (var rowIndex = 0; rowIndex < _tiles.GetLength(1); ++rowIndex)
            {
                for (var colIndex = 0; colIndex < _tiles.GetLength(0); ++colIndex)
                {
                    _tiles[colIndex, rowIndex].UpdateStatus(tiles[colIndex, rowIndex]);
                }
            }
        }

        [SerializeField]
        GraphicRaycaster? GraphicRaycaster;


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
