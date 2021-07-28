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
        [SerializeField]
        Gem? Gem;

        internal TileStatus TileStatus => TileData.TileStatus;

        internal Coordinate Coordinate { get; private set; }

        internal TileData TileData { get; private set; }

        void Awake()
        {
            Assert.IsNotNull(Gem, "Gem != null");
        }

        internal void ClearSelection()
        {
            Gem!.QuitWorking();
        }

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
            await Gem!.NewGem();
        }

        internal async Task Popup(GemColorType gemColorType)
        {
            UpdateStatus(new TileStatus(TileStatusType.Contain, gemColorType));
            await Gem!.Popup();
        }

        internal void SetEvents(UserPlayer userPlayer)
        {
            TemporaryGemTile.SetEvents(userPlayer, Gem!.Image!, Gem!, gameObject, this);
        }

        internal async Task Slide()
        {
            await Gem!.Slide(UpdateStatus);
        }

        internal void UpdateStatus(TileStatus newTileStatus)
        {
            TileData = new TileData(Coordinate, newTileStatus);

            newTileStatus.TileStatusType.Switch(
                () => { gameObject.SetActive(value: false); },
                () => { Gem!.Image!.enabled = false; },
                () =>
                {
                    Assert.IsNotNull(newTileStatus.GemColorType, "newTileStatus.NewGemColorType != null");
                    Gem!.UpdateStatus(newTileStatus);
                }
            );
        }

        internal async Task Vanish()
        {
            await Gem!.Vanish(UpdateStatus);
        }
    }
}
