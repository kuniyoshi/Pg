#nullable enable
using System;
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

        Sequence? _sequence;

        void Awake()
        {
            Assert.IsNotNull(Image, "Image != null");
        }

        public async Task NewGem()
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
        }

        public void UpdateStatus(Sprite sprite)
        {
            Image!.sprite = sprite;
            Image!.enabled = true;
        }

        public async Task Popup()
        {
            Image!.enabled = true;
            Image!.GetComponentStrictly<RectTransform>().localScale = Vector3.zero;
            await Image!.GetComponentStrictly<RectTransform>()
                .DOScale(Vector3.one, duration: 0.05f)
                .AsyncWaitForCompletion();
        }

        public async Task Slide(Action<TileStatus> updateStatus)
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

        public async Task Vanish(Action<TileStatus> updateStatus)
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
    }
}
