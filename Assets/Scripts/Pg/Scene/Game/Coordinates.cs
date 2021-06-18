#nullable enable
using System;
using Pg.App.Util;
using Pg.Etc.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    public class Coordinates
        : MonoBehaviour
    {
        [SerializeField]
        float OffsetSize;

        [SerializeField]
        GameObject? HexiagonPrefab;

        [SerializeField]
        RectTransform? Canvas;

        GameObject[]? _hexiagons;
        const int Size = 30;
        static Vector2Int DeltaTopLeft { get; } = new Vector2Int(-3, -2);

        static Vector2 TopLeftOffset { get; } = new Vector2(
            2 * Size * DeltaTopLeft.x * 3f / 4f,
            -Mathf.Sqrt(3f) * Size * DeltaTopLeft.y
        );

        void Awake()
        {
            Assert.IsNotNull(HexiagonPrefab, "HexiagonPrefab != null");
            Assert.IsNotNull(Canvas, "Canvas != null");
        }

        void Start()
        {
            var positions =new Vector2[TileSize.RowSize*TileSize.ColSize];
            var offsetY = Mathf.Sqrt(3f) * Size * 0.5f;
            var intervalX = 2 * Size * 3f / 4f;
            var intervalY = Mathf.Sqrt(3f) * Size;

            for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
            {
                for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
                {
                    var offset = colIndex % 2 == 0 ? 0f : offsetY;
                    var position = new Vector2(colIndex * intervalX, -intervalY * rowIndex + offset);
                    positions[rowIndex * TileSize.ColSize + colIndex] = TopLeftOffset + position;
                }
            }

            _hexiagons = new GameObject[TileSize.RowSize * TileSize.ColSize];

            for (var rowIndex = 0; rowIndex < TileSize.RowSize; ++rowIndex)
            {
                for (var colIndex = 0; colIndex < TileSize.ColSize; ++colIndex)
                {
                    var position = positions[rowIndex * TileSize.ColSize + colIndex];
                    var hexiagon = Instantiate(HexiagonPrefab!, Canvas);
                    hexiagon.name = $"{HexiagonPrefab!.name}({colIndex}, {rowIndex})";
                    var text = hexiagon.GetComponentInChildrenStrictly<Text>();
                    text.text = $"({colIndex}, {rowIndex})";
                    var rectTransform = hexiagon.GetComponentStrictly<RectTransform>();
                    rectTransform.localPosition = position;

                    _hexiagons[rowIndex * TileSize.ColSize + colIndex] = hexiagon;
                }
            }
        }


        public void ApplyTiles(TileStatus[,] tiles)
        {
            throw new NotImplementedException();
        }
    }
}
