#nullable enable
using System.Threading.Tasks;
using Pg.App.Util;
using Pg.Data.Simulation;
using Pg.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Tile
        : MonoBehaviour
    {
        internal TileStatusType? TileStatusType { get; private set; }

        internal TileStatus TileStatus => TileData.TileStatus;

        internal Coordinate Coordinate { get; private set; }

        internal TileData TileData { get; private set; }

        internal void Initialize(int colIndex, int rowIndex, Vector2 localPosition)
        {
            Coordinate = new Coordinate(colIndex, rowIndex);
            TileData = new TileData(Coordinate, TileStatus.Empty);
            var text = this.GetComponentInChildrenStrictly<Text>();
            text.text = Coordinate.ToString();
            var rectTransform = this.GetComponentStrictly<RectTransform>();
            rectTransform.localPosition = localPosition;
        }

        internal async Task NewGem(GemColorType gemColorType)
        {
            UpdateStatus(new TileStatus(TileStatusType.Contain, gemColorType));
        }

        internal void Popup(GemColorType gemColorType)
        {
            var newTileStatus = new TileStatus(TileStatusType.Contain, gemColorType);
            TileData = new TileData(Coordinate, newTileStatus);

            // newTileStatus.TileStatusType.Switch(
            //     () => { gameObject.SetActive(value: false); },
            //     () => Gem!.MakeEmpty(),
            //     () =>
            //     {
            //         Assert.IsNotNull(newTileStatus.GemColorType, "newTileStatus.NewGemColorType != null");
            //         Gem!.UpdateStatus(newTileStatus);
            //     }
            // );
        }

        internal void SetEvents(UserPlayer userPlayer)
        {
            // TemporaryGemTile.SetEvents(userPlayer, this, Gem!, gameObject);
        }

        internal void UpdateStatus(TileStatus newTileStatus)
        {
            TileData = new TileData(Coordinate, newTileStatus);

            // newTileStatus.TileStatusType.Switch(
            //     () => { gameObject.SetActive(value: false); },
            //     () => Gem!.MakeEmpty(),
            //     () =>
            //     {
            //         Assert.IsNotNull(newTileStatus.GemColorType, "newTileStatus.NewGemColorType != null");
            //         Gem!.UpdateStatus(newTileStatus);
            //     }
            // );
        }
        internal void UpdateStatus2(TileStatusType newTileStatusType)
        {
            TileStatusType = newTileStatusType;
        }
    }
}
