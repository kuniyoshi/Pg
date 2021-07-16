#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pg.Rule;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game.Internal
{
    internal class GameEndDirection
        : MonoBehaviour
    {
        static async UniTask DoBounceDown(Text text,
                                          CancellationToken cancellationToken, float delay, float duration)
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
        Text[]? SucceedLetters;

        [SerializeField]
        float[]? SucceedDurations;

        [SerializeField]
        float[]? SucceedDelays;

        [SerializeField]
        Vector3[]? SucceedInitialVectors;

        void Awake()
        {
            Assert.IsNotNull(FailureLetters, "FailureLetters != null");
            Assert.IsTrue(
                FailureLetters!.Any() && FailureLetters!.All(text => text != null),
                "FailureLetters!.Any() && FailureLetters!.All(text => text != null)"
            );
            Assert.IsNotNull(SucceedLetters, "SucceedLetters != null");
            Assert.IsTrue(
                SucceedLetters!.Any() && SucceedLetters!.All(text => text != null),
                "SucceedLetters!.Any() && SucceedLetters!.All(text => text != null)"
            );

            Assert.IsNotNull(SucceedDurations, "SucceedDurations != null");
            Assert.IsNotNull(SucceedDelays, "SucceedDelays != null");
            Assert.IsNotNull(SucceedInitialVectors, "SucceedInitialVectors != null");

            foreach (var letter in FailureLetters!)
            {
                letter.gameObject.SetActive(value: false);
            }

            foreach (var letter in SucceedLetters!)
            {
                letter.gameObject.SetActive(value: false);
            }
        }

        internal UniTask PlayFailure()
        {
            return Play(JudgeResult.Failure);
        }

        internal UniTask PlaySucceed()
        {
            return Play(JudgeResult.Succeed);
        }

        async UniTask DoHoming(Text letter, float workDuration, Vector3 initialVelocity, float homingDelay)
        {
            static IEnumerator Work(RectTransform r, float duration, Vector3 velocity, float delay)
            {
                var waitTime = delay;

                while (waitTime > 0f)
                {
                    waitTime = waitTime - Time.deltaTime;
                    yield return null;
                }

                r.gameObject.SetActive(value: true);
                var lastPosition = r.position;
                var initialPosition = r.position;
                initialPosition.x = initialPosition.x + 300f;
                r.position = initialPosition;

                var period = duration;

                while (period > 0f)
                {
                    var diff = lastPosition - r.position;
                    var acceleration = (diff - velocity * period) * 2f / (period * period);
                    period = period - Time.deltaTime;

                    yield return null;

                    velocity = velocity + acceleration * Time.deltaTime;
                    r.position = r.position + velocity * Time.deltaTime;
                }

                r.position = lastPosition;
            }

            await Work(letter.rectTransform, workDuration, initialVelocity, homingDelay);
        }

        UniTask OnFailure()
        {
            var token = this.GetCancellationTokenOnDestroy();

            var delays = new[] {0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f};
            var duration = 0.2f;

            var tasks = new List<UniTask>();

            for (var letterIndex = 0; letterIndex < FailureLetters!.Length; ++letterIndex)
            {
                var letter = FailureLetters![letterIndex];
                var delay = delays[letterIndex];
                tasks.Add(DoBounceDown(letter, token, delay, duration));
            }

            return UniTask.WhenAll(tasks);
        }

        UniTask OnSucceed()
        {
            var tasks = new List<UniTask>();

            for (var letterIndex = 0; letterIndex < SucceedLetters!.Length; ++letterIndex)
            {
                var letter = SucceedLetters![letterIndex];
                var duration = SucceedDurations![letterIndex];
                var delay = SucceedDelays![letterIndex];
                var initialVector = SucceedInitialVectors![letterIndex];

                tasks.Add(DoHoming(letter, duration, initialVector, delay));
            }

            return UniTask.WhenAll(tasks);
        }

        UniTask Play(JudgeResult judgeResult)
        {
            return judgeResult.Switch(
                () => UniTask.CompletedTask,
                OnFailure,
                OnSucceed
            );
        }
    }
}
