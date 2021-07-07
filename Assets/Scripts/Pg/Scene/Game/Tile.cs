#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        List<NewGemColorTypeVsSprite>? Map;

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

        public void ClearSelection()
        {
            QuitWorking();
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

        public async Task NewGem(GemColorType gemColorType)
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            UpdateStatus(new TileStatus(TileStatusType.Contain, gemColorType));
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
        }

        public async Task Popup(GemColorType gemColorType)
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            UpdateStatus(new TileStatus(TileStatusType.Contain, gemColorType));
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
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

        public async Task Slide()
        {
            Image!.enabled = true;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.zero, duration: 0.05f)
                .OnComplete(() =>
                {
                    UpdateStatus(TileStatus.Empty);
                    Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.one;
                })
                .AsyncWaitForCompletion();
        }

        public void UpdateStatus(TileStatus newTileStatus)
        {
            TileData = new TileData(Coordinate, newTileStatus);

            newTileStatus.TileStatusType.Switch(
                () => { gameObject.SetActive(value: false); },
                () => { Image!.enabled = false; },
                () =>
                {
                    Assert.IsNotNull(newTileStatus.GemColorType, "newTileStatus.NewGemColorType != null");
                    var statusVsSprite = Map!.First(pair => pair.First.Convert() == newTileStatus.GemColorType);
                    Image!.sprite = statusVsSprite.Second;
                    Image!.enabled = true;
                }
            );
        }

        public async Task Vanish()
        {
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.zero, duration: 0.1f)
                .OnComplete(() =>
                {
                    UpdateStatus(TileStatus.Empty);
                    Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.one;
                })
                .AsyncWaitForCompletion();
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
            foreach (var newGemColorType in GemColorType.Values)
            {
                Assert.IsTrue(
                    Map!.Any(item => item.First.Convert() == newGemColorType),
                    "Map!.Any(item => item.First.Convert() == newGemColorType)"
                );
            }

            Assert.IsTrue(
                Map!.All(pair => pair.Second != null),
                "Map.All(pair => pair.Second != null)"
            );
        }

        [Serializable]
        public class NewGemColorTypeVsSprite
            : Pair<SerializableGemColorType, Sprite>
        {
            public NewGemColorTypeVsSprite(SerializableGemColorType first, Sprite second)
                : base(first, second)
            {
            }
        }
    }
}
