#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Pg.App.Util;
using Pg.Puzzle;
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

        Sequence? _sequence;

        public TileStatus TileStatus => TileData.TileStatus;

        public Coordinate Coordinate { get; private set; }

        public TileData TileData { get; private set; }

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            ZzDebugAssertMapValue();
        }

        public void Initialize(int colIndex, int rowIndex, Vector2 localPosition)
        {
            Coordinate = new Coordinate(colIndex, rowIndex);
            TileData = new TileData(Coordinate, TileStatus.Empty);
            var text = this.GetComponentInChildrenStrictly<Text>();
            text.text = Coordinate.ToString();
            var rectTransform = this.GetComponentStrictly<RectTransform>();
            rectTransform.localPosition = localPosition;
        }

        public void SetEvents(UserPlayer userPlayer)
        {
            Image!.OnPointerDownAsObservable()
                .Subscribe(data =>
                {
                    var didStart = userPlayer.StartTransactionIfNotAlready(this);

                    if (didStart)
                    {
                        SelectFirstTime();
                        MakeWorking();
                    }
                })
                .AddTo(gameObject);
            Image!.OnPointerExitAsObservable()
                .Subscribe(data =>
                {
                    var isSelected = userPlayer.IsSelected(this);

                    if (!isSelected)
                    {
                        QuitWorking();
                    }
                })
                .AddTo(gameObject);
            Image!.OnPointerEnterAsObservable()
                .Subscribe(data =>
                {
                    var didAdd = userPlayer.TryAddTransaction(this);

                    if (didAdd)
                    {
                        MakeWorking();
                    }
                })
                .AddTo(gameObject);
            Image!.OnPointerUpAsObservable()
                .Subscribe(data => userPlayer.CompleteTransaction())
                .AddTo(gameObject);
        }

        public void UpdateStatus(TileStatus newStatus)
        {
            TileData = new TileData(Coordinate, newStatus);
            var statusVsSprite = Map!.FirstOrDefault(pair => pair.First == newStatus);

            if (statusVsSprite != null)
            {
                Image!.sprite = statusVsSprite.Second;
                Image!.enabled = true;

                return;
            }

            if (newStatus == TileStatus.Empty)
            {
                Image!.enabled = false;

                return;
            }

            Assert.AreEqual(TileStatus.Closed, newStatus);
            gameObject.SetActive(value: false);
        }

        void MakeWorking()
        {
            if (_sequence != null)
            {
                return;
            }

            var rectTransform = Image!.GetComponentStrictly<RectTransform>();
            // rectTransform.DOShakePosition(0.2f, Vector3.left, 10, 0, false);
            var currentX = rectTransform.position.x;
            _sequence = DOTween.Sequence()
                // .Append(rectTransform.DOMoveX(endValue: 10f + currentX, duration: 0.1f))
                // .Append(rectTransform.DOMoveX(endValue: -10f + currentX, duration: 0.2f))
                // .Append(rectTransform.DOMoveX(endValue: currentX, duration: 0.1f))
                .Append(rectTransform.DOShakePosition(0.5f, 10f * Vector3.left, 100, 0, false).SetRelative())
                .SetLoops(loops: -1);
                // .OnComplete(() => rectTransform.DOMoveX(currentX, 0f));
            _sequence.Play();
        }

        public void ClearSelection()
        {
            QuitWorking();
        }

        void QuitWorking()
        {
            _sequence?.Kill(complete: true);
        }

        void SelectFirstTime()
        {
            Image!.GetComponentStrictly<RectTransform>()
                .DOScale(1.1f * Vector3.one, duration: 0.5f)
                .SetLoops(loops: 2, LoopType.Yoyo);
        }

        [Conditional("DEBUG")]
        void ZzDebugAssertMapValue()
        {
            foreach (var value in Enum.GetValues(typeof(TileStatus)))
            {
                var tileStatus = (TileStatus) value;

                Assert.IsTrue(
                    tileStatus == TileStatus.Closed
                    || tileStatus == TileStatus.Empty
                    || Map!.Any(item => item.First == tileStatus),
                    "Invalid Status Found"
                );
            }

            Assert.IsTrue(
                Map!.All(pair => pair.Second != null),
                "Map.All(pair => pair.Second != null)"
            );
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
    }
}
