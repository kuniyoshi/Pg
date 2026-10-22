#nullable enable
using Pg.App.Util;
using Pg.Data.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Tile : MonoBehaviour
    {
        internal Coordinate Coordinate { get; private set; }

        internal void Initialize(int colIndex, int rowIndex, Vector2 localPosition)
        {
            Coordinate = new Coordinate(colIndex, rowIndex);
            this.GetComponentInChildrenStrictly<Text>().text = Coordinate.ToString();
            this.GetComponentStrictly<RectTransform>().localPosition = localPosition;
        }

        internal void SetClosed(bool closed)
        {
            gameObject.SetActive(!closed);
        }
    }
}
