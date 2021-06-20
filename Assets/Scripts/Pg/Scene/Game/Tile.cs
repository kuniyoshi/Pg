#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Pg.App.Util;
using Pg.Etc.Puzzle;
using UniRx;
using UniRx.Triggers;
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

        public TileStatus TileStatus { get; private set; }

        public int ColIndex { get; private set; }
        public int RowIndex { get; private set; }

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
            TileStatus = newStatus;
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
            RowIndex = rowIndex;
            ColIndex = colIndex;
            var text = this.GetComponentInChildrenStrictly<Text>();
            text.text = $"({colIndex}, {rowIndex})";
            var rectTransform = this.GetComponentStrictly<RectTransform>();
            rectTransform.localPosition = localPosition;
        }

        void Select()
        {
            this.GetComponentStrictly<RectTransform>().DOScale(1.1f * Vector3.one, 0.5f).SetLoops(2, LoopType.Yoyo);
        }

        public void SetEvents(UserPlayer userPlayer)
        {
            Image!.OnPointerDownAsObservable()
                .Subscribe(data =>
                {
                    var didStart = userPlayer.StartTransactionIfNotAlready(this);

                    if (didStart)
                    {
                        Select();
                    }
                })
                .AddTo(gameObject);
            Image!.OnPointerEnterAsObservable()
            .Subscribe(data =>
            {
                userPlayer.TryAddTransaction(this);
            })
            .AddTo(gameObject);
            Image!.OnPointerUpAsObservable()
                .Subscribe(data => { userPlayer.CompleteTransaction(); })
                .AddTo(gameObject);
        }
    }
}
