#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pg.App.Util;
using Pg.Data.Simulation;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Gem : MonoBehaviour
    {
        [SerializeField]
        Image Image = null!;

        [SerializeField]
        List<GemColorTypeVsSprite> Map = null!;

        [SerializeField]
        float MoveDuration;

        [SerializeField]
        float VanishDuration;

        [SerializeField]
        float PopupDuration;

        Tile _tile = null!;
        Tween? _moveTween;
        Tween? _selectionTween;

        internal Coordinate Coordinate => _tile.Coordinate;
        internal GemColorType Color { get; private set; } = null!;
        internal bool IsMoving => _moveTween.IsActive() && _moveTween.IsPlaying();

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            Assert.IsTrue(GemColorType.Values.All(color =>
                Map.Any(item => item.First.Convert() == color && item.Second != null)),
                "Every gem color must have a sprite");
        }

        internal void Initialize(Tile tile, GemColorType color, UserPlayer userPlayer)
        {
            _tile = tile;
            Color = color;
            transform.position = tile.transform.position;
            Image.sprite = new GemColor(color).SelectSprite(Map);
            Bind(userPlayer);
        }

        void Bind(UserPlayer userPlayer)
        {
            Image.OnPointerDownAsObservable()
                .Subscribe(_ =>
                {
                    if (userPlayer.StartTransactionIfNotAlready(this))
                    {
                        _selectionTween = Image.rectTransform
                            .DOScale(1.1f * Vector3.one, duration: 0.5f)
                            .SetLoops(loops: 2, LoopType.Yoyo)
                            .SetLink(gameObject);
                        var selectionTween = _selectionTween;
                        selectionTween.onKill += () =>
                        {
                            if (ReferenceEquals(_selectionTween, selectionTween)) _selectionTween = null;
                        };
                    }
                }).AddTo(gameObject);
            Image.OnPointerEnterAsObservable()
                .Subscribe(_ => userPlayer.TryAddTransaction(this))
                .AddTo(gameObject);
            Image.OnPointerUpAsObservable()
                .Subscribe(_ => userPlayer.CompleteTransaction())
                .AddTo(gameObject);
        }

        internal void QuitWorking()
        {
            if (_moveTween.IsActive()) _moveTween.Complete(withCallbacks: true);
            if (_selectionTween.IsActive()) _selectionTween.Kill();
            Image.rectTransform.localScale = Vector3.one;
            transform.position = _tile.transform.position;
        }

        internal void Swap(Gem other)
        {
            QuitWorking();
            other.QuitWorking();
            (_tile, other._tile) = (other._tile, _tile);
            MoveToTile().Forget();
            other.MoveToTile().Forget();
        }

        UniTask MoveToTile()
        {
            var tween = transform.DOMove(_tile.transform.position, MoveDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(gameObject);
            _moveTween = tween;
            tween.onKill += () =>
            {
                if (ReferenceEquals(_moveTween, tween)) _moveTween = null;
            };
            return tween.ToUniTask();
        }

        internal UniTask SlideTo(Tile tile)
        {
            QuitWorking();
            _tile = tile;
            return MoveToTile();
        }

        internal async UniTask Vanish()
        {
            QuitWorking();
            Image.raycastTarget = false;
            await Image.rectTransform.DOScale(Vector3.zero, VanishDuration)
                .SetLink(gameObject)
                .ToUniTask();
            Destroy(gameObject);
        }

        internal async UniTask Popup()
        {
            Image.rectTransform.localScale = Vector3.zero;
            await Image.rectTransform.DOScale(Vector3.one, PopupDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(gameObject)
                .ToUniTask();
        }
    }

    [Serializable]
    internal class GemColorTypeVsSprite : Pair<SerializableGemColorType, Sprite>
    {
        internal GemColorTypeVsSprite(SerializableGemColorType first, Sprite second)
            : base(first, second)
        {
        }
    }
}
