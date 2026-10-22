#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pg.App.Util;
using Pg.Data.Simulation;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Gem2
        : MonoBehaviour
    {
        [SerializeField]
        Image Image = null!;

        [SerializeField]
        List<GemColorTypeVsSprite> Map = null!;

        TileBuffer Tile { get; } = new TileBuffer();

        [SerializeField]
        float DebugAlpha;

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            ZzDebugAssertMapValue();
        }

        void Start()
        {
            Image.DOFade(DebugAlpha, 0f);
        }

        internal void Initialize(Tile tile, GemColorType gemColorType)
        {
            Tile.Initialize(tile);
            SetPosition(Tile.Current!);
            Image.sprite = new GemColor(gemColorType).SelectSprite(Map);
        }

        void SetPosition(Tile tile)
        {
            transform.position = tile.transform.position;
        }

        internal Coordinate GetStableCoordinate()
        {
            Assert.IsTrue(Tile.IsStable, "TileBuffer.IsStable");
            return Tile.Current!.Coordinate;
        }

        [Conditional("DEBUG")]
        void ZzDebugAssertMapValue()
        {
            foreach (var newGemColorType in GemColorType.Values)
            {
                Assert.IsTrue(
                    Map.Any(item => item.First.Convert() == newGemColorType),
                    "Map!.Any(item => item.First.Convert() == newGemColorType)"
                );
            }

            Assert.IsTrue(
                Map.All(pair => pair.Second != null),
                "Map.All(pair => pair.Second != null)"
            );
        }

        class TileBuffer
        {
            internal Tile? Current { get; private set; }
            internal Tile? Next { get; private set; }
            public bool IsStable => Next is null;

            internal void Initialize(Tile tile)
            {
                Current = tile;
            }

            internal void SetNext(Tile next)
            {
                Next = next;
            }

            public void Commit()
            {
                (Current, Next) = (Next, null);
            }
        }

        public void QuitWorking()
        {
            Move.Finish(withCallbacks: false);
            SetPosition(Tile.Current!);
            // Image.GetComponentStrictly<RectTransform>().localPosition = Vector3.zero;
        }

        public void Bind(UserPlayer userPlayer)
        {
            Image.OnPointerDownAsObservable()
                .Subscribe(data =>
                    {
                        // UnityEngine.Debug.Log($"### on pointer DOWN: {gameObject.name}");
                        var didStart = userPlayer.StartTransactionIfNotAlready(this);

                        if (didStart)
                        {
                            SelectFirstTime();
                            MakeWorking();
                        }
                    }
                )
                .AddTo(gameObject);
            Image.OnPointerExitAsObservable()
                .Subscribe(data =>
                    {
                        if (Move.IsInMove)
                        {
                            return;
                        }

                        // UnityEngine.Debug.Log($"### on pointer EXIT: {gameObject.name}");

                        var isSelected = userPlayer.IsSelected(this);

                        if (!isSelected)
                        {
                            QuitWorking();
                        }
                    }
                )
                .AddTo(gameObject);
            Image.OnPointerEnterAsObservable()
                .Subscribe(data =>
                    {
                        if (Move.IsInMove)
                        {
                            return;
                        }

                        // UnityEngine.Debug.Log($"### on pointer ENTER: {gameObject.name}");

                        var didAdd = userPlayer.TryAddTransaction(this);

                        if (didAdd)
                        {
                            MakeWorking();
                        }
                    }
                )
                .AddTo(gameObject);
            Image.OnPointerUpAsObservable()
                .Subscribe(data =>
                    {
                        // UnityEngine.Debug.Log($"### on pointer UP: {gameObject.name}");

                        userPlayer.CompleteTransaction();
                    }
                )
                .AddTo(gameObject);
        }

        void SelectFirstTime()
        {
            Image.GetComponentStrictly<RectTransform>()
                .DOScale(1.1f * Vector3.one, duration: 0.5f)
                .SetLoops(loops: 2, LoopType.Yoyo);
        }

        void MakeWorking()
        {
            if (Move.IsInShake)
            {
                return;
            }

            // Move.SetShake(
            //     DOTween.Sequence()
            //         .Append(
            //             Image
            //                 .GetComponentStrictly<RectTransform>()
            //                 .DOShakePosition(duration: 0.5f, 10f * Vector3.left, vibrato: 100, randomness: 0)
            //                 .SetRelative()
            //         )
            //         .SetLoops(loops: -1)
            // );
        }

        public UniTask Swap(Gem2 other)
        {
            Assert.IsNotNull(Tile.Current, "Tile.Current != null");
            Assert.IsNotNull(other.Tile.Current, "other.Tile.Current != null");
            var (myTile, otherTile) = (Tile.Current!, other.Tile.Current!);
            Tile.SetNext(otherTile);
            other.Tile.SetNext(myTile);

            return UniTask.WhenAll(
                MoveToNextTile(),
                other.MoveToNextTile()
            );
        }

        UniTask MoveToNextTile()
        {
            Assert.IsNotNull(Tile.Next, "Tile.Next != null");
            Assert.IsFalse(Move.IsInMove, "Move.IsInMove");
            return Move.SetMove(
                DOTween.Sequence()
                    .Append(transform.DOMove(Tile.Next!.transform.position, MoveDuration).SetEase(Ease.OutBounce))
                    .OnComplete(Tile.Commit)
            );
        }

        [SerializeField]
        float MoveDuration;

        [SerializeField]
        float VanishDuration;

        [SerializeField]
        float PopupDuration;

        MoveMemory Move { get; } = new MoveMemory();

        class MoveMemory
        {
            internal bool IsInMove => _moveSequence.IsActive() && _moveSequence.IsPlaying();
            Sequence? _moveSequence;
            Sequence? _shakeSequence;

            internal UniTask SetMove(Sequence moveSequence)
            {
                _moveSequence = moveSequence;
                moveSequence.onKill += () =>
                {
                    if (ReferenceEquals(_moveSequence, moveSequence))
                    {
                        _moveSequence = null;
                    }
                };
                return moveSequence.ToUniTask();
            }

            internal bool IsInShake => _shakeSequence.IsActive() && _shakeSequence.IsPlaying();

            public void SetShake(Sequence shakeSequence)
            {
                _shakeSequence = shakeSequence;
                shakeSequence.onKill += () =>
                {
                    if (ReferenceEquals(_shakeSequence, shakeSequence))
                    {
                        _shakeSequence = null;
                    }
                };
            }

            public void Finish(bool withCallbacks)
            {
                if (_shakeSequence.IsActive())
                {
                    _shakeSequence.Complete(withCallbacks);
                }

                if (_moveSequence.IsActive())
                {
                    _moveSequence.Complete(withCallbacks);
                }
            }

            public UniTask SetVanish(Tweener scaleSequence)
            {
                return scaleSequence.ToUniTask();
            }
        }

        public UniTask Vanish()
        {
            return Move.SetVanish(
                Image.GetComponentStrictly<RectTransform>()
                    .DOScale(Vector3.zero, VanishDuration)
            );
        }

        public async Task SlideTo(Tile tile)
        {
            Tile.SetNext(tile);
            await MoveToNextTile();
        }

        public async UniTask Popup()
        {

            Image.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            await Image.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, PopupDuration)
                .SetEase(Ease.OutBounce)
                .ToUniTask();
        }
    }
}
