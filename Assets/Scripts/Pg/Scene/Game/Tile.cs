#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Pg.App.Util;
using Pg.Etc.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    public class Tile
        : MonoBehaviour
    {
        [SerializeField]
        List<TileStatusVsSprite>? Map;

        [SerializeField]
        Image? Image;

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            AssertMapValue();
        }

        #region debug

        [Conditional("DEBUG")]
        void AssertMapValue()
        {
            foreach (var value in Enum.GetValues(typeof(TileStatus)))
            {
                var tileStatus = (TileStatus) value;

                Assert.IsTrue(
                    tileStatus == TileStatus.Closed || Map!.Any(item => item.First == tileStatus),
                    "tileStatus == TileStatus.Closed || Map!.Any(item => item.First == tileStatus)"
                );
            }

            Assert.IsTrue(
                Map!.All(pair => pair.Second != null),
                "Map.All(pair => pair.Second != null)"
            );
        }

        #endregion

        public void UpdateStatus(TileStatus newStatus)
        {
            var statusVsSprite = Map!.FirstOrDefault(pair => pair.First == newStatus);

            if (statusVsSprite != null)
            {
                Image!.sprite = statusVsSprite.Second;
            }
            else
            {
                Assert.AreEqual(TileStatus.Closed, newStatus);
                gameObject.SetActive(false);
            }
        }

        [Serializable]
        public class TileStatusVsSprite
            : Pair<TileStatus, Sprite>
        {
            public TileStatusVsSprite(TileStatus first, Sprite second)
                : base(first, second)
            {
            }
        }

        public void Initialize(int colIndex, int rowIndex, Vector2 localPosition)
        {
            var text = this.GetComponentInChildrenStrictly<Text>();
            text.text = $"({colIndex}, {rowIndex})";
            var rectTransform = this.GetComponentStrictly<RectTransform>();
            rectTransform.localPosition = localPosition;
        }

        public void Select()
        {
            this.GetComponentStrictly<RectTransform>().DOScale(1.5f * Vector3.one, 0.5f).SetRelative().SetLoops(1, LoopType.Yoyo);
        }
    }
}
