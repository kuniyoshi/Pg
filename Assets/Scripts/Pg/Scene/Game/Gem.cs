#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Pg.App.Util;
using Pg.Data.Simulation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class Gem
        : MonoBehaviour
    {
        [SerializeField]
        internal Image? Image;

        [SerializeField]
        List<NewGemColorTypeVsSprite>? Map;

        Sequence? _sequence;

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
            Assert.IsNotNull(Map, "Map != null");
            ZzDebugAssertMapValue();
        }

        internal void MakeEmpty()
        {
            Image!.enabled = false;
        }

        internal void MakeWorking()
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

        internal async Task NewGem()
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
        }

        internal async Task Popup()
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
        }

        internal void QuitWorking()
        {
            _sequence?.Kill(complete: false);
            Image!.GetComponentStrictly<RectTransform>().localPosition = Vector3.zero;
        }

        internal void SelectFirstTime()
        {
            Image!.GetComponentStrictly<RectTransform>()
                .DOScale(1.1f * Vector3.one, duration: 0.5f)
                .SetLoops(loops: 2, LoopType.Yoyo);
        }

        internal async Task Slide(Action<TileStatus> updateStatus)
        {
            Image!.enabled = true;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.zero, duration: 0.05f)
                .OnComplete(() =>
                    {
                        updateStatus(TileStatus.Empty);
                        Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.one;
                    }
                )
                .AsyncWaitForCompletion();
        }

        internal void UpdateStatus(TileStatus newTileStatus)
        {
            Image!.sprite = Map!.First(pair => pair.First.Convert() == newTileStatus.GemColorType)
                .Second;
            Image!.enabled = true;
        }

        internal async Task Vanish(Action<TileStatus> updateStatus)
        {
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.zero, duration: 0.1f)
                .OnComplete(() =>
                    {
                        updateStatus(TileStatus.Empty);
                        Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.one;
                    }
                )
                .AsyncWaitForCompletion();
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
    }

    [Serializable]
    internal class NewGemColorTypeVsSprite
        : Pair<SerializableGemColorType, Sprite>
    {
        internal NewGemColorTypeVsSprite(SerializableGemColorType first, Sprite second)
            : base(first, second)
        {
        }
    }
}
