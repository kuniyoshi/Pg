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

        public TileStatusType TileStatusType => TileData.TileStatusType;

        public Coordinate Coordinate { get; private set; }

        public TileData TileData { get; private set; }

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            ZzDebugAssertMapValue();
        }

        public void ClearSelection()
        {
            QuitWorking();
        }

        public void Initialize(int colIndex, int rowIndex, Vector2 localPosition)
        {
            Coordinate = new Coordinate(colIndex, rowIndex);
            TileData = new TileData(Coordinate, TileStatusType.Empty);
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

        public void UpdateStatus(TileStatusType newStatusType)
        {
            TileData = new TileData(Coordinate, newStatusType);
            var statusVsSprite = Map!.FirstOrDefault(pair => pair.First == newStatusType);

            if (statusVsSprite != null)
            {
                Image!.sprite = statusVsSprite.Second;
                Image!.enabled = true;

                return;
            }

            if (newStatusType == TileStatusType.Empty)
            {
                Image!.enabled = false;

                return;
            }

            Assert.AreEqual(TileStatusType.Closed, newStatusType);
            gameObject.SetActive(value: false);
        }

        void MakeWorking()
        {
            if (_sequence != null)
            {
                return;
            }

            _sequence = DOTween.Sequence()
                .Append(
                    Image!
                        .GetComponentStrictly<RectTransform>()
                        .DOShakePosition(duration: 0.5f, 10f * Vector3.left, vibrato: 100, randomness: 0)
                        .SetRelative()
                )
                .SetLoops(loops: -1);
            _sequence.Play();
        }

        void QuitWorking()
        {
            _sequence?.Kill(complete: false);
            Image!.GetComponentStrictly<RectTransform>().localPosition = Vector3.zero;
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
            foreach (var value in Enum.GetValues(typeof(TileStatusType)))
            {
                var tileStatus = (TileStatusType) value;

                Assert.IsTrue(
                    tileStatus == TileStatusType.Closed
                    || tileStatus == TileStatusType.Empty
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
            : Pair<TileStatusType, Sprite>
        {
            public TileStatusVsSprite(TileStatusType first, Sprite second)
                : base(first, second)
            {
            }
        }
    }
}
