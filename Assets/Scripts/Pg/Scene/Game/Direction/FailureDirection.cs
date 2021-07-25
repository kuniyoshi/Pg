#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game.Direction
{
    internal class FailureDirection
        : MonoBehaviour
    {
        static async UniTask DoBounceDown(Text text,
                                          CancellationToken cancellationToken,
                                          float delay,
                                          float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
            text.gameObject.SetActive(value: true);
            await text.rectTransform.DOMoveY(endValue: 300f, duration: 0f)
                .SetRelative()
                .WithCancellation(cancellationToken);
            await text.rectTransform.DOMoveY(endValue: -300f, duration)
                .SetRelative()
                .SetEase(Ease.OutBounce)
                .WithCancellation(cancellationToken);
        }

        [SerializeField]
        Text[]? FailureLetters;

        [SerializeField]
        Image? Background;

        void Awake()
        {
            Assert.IsNotNull(FailureLetters, "FailureLetters != null");
            Assert.IsTrue(
                FailureLetters!.Any() && FailureLetters!.All(text => text != null),
                "FailureLetters!.Any() && FailureLetters!.All(text => text != null)"
            );

            Assert.IsNotNull(Background, "Background != null");

            foreach (var letter in FailureLetters!)
            {
                letter.gameObject.SetActive(value: false);
            }

            Background!.enabled = false;
            Background!.DOFade(endValue: 0f, duration: 0f);
        }

        internal UniTask PlayFailure()
        {
            Background!.enabled = true;
            Background!.DOFade(endValue: 0.5f, duration: 0.2f);

            Text[] failureLetters = FailureLetters!;
            var token = this.GetCancellationTokenOnDestroy();

            var delays = new[] {0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f};
            var duration = 0.2f;

            var tasks = new List<UniTask>();

            for (var letterIndex = 0; letterIndex < failureLetters.Length; ++letterIndex)
            {
                var letter = failureLetters[letterIndex];
                var delay = delays[letterIndex];
                tasks.Add(DoBounceDown(letter, token, delay, duration));
            }

            return UniTask.WhenAll(tasks);
        }
    }
}
