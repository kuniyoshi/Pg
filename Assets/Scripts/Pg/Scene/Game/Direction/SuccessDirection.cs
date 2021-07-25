#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game.Direction
{
    internal class SuccessDirection
        : MonoBehaviour
    {
        static async UniTask DoHoming(Text letter,
                                      float workDuration,
                                      Vector3 initialVelocity,
                                      float homingDelay)
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

        [SerializeField]
        Text[]? SuccessLetters;

        [SerializeField]
        float[]? SuccessDurations;

        [SerializeField]
        float[]? SuccessDelays;

        [SerializeField]
        Vector3[]? SuccessInitialVectors;

        [SerializeField]
        Image? Background;

        void Awake()
        {
            Assert.IsNotNull(SuccessLetters, "SuccessLetters != null");
            Assert.IsTrue(
                SuccessLetters!.Any() && SuccessLetters!.All(text => text != null),
                "SuccessLetters!.Any() && SuccessLetters!.All(text => text != null)"
            );

            Assert.IsNotNull(SuccessDurations, "SuccessDurations != null");
            Assert.IsNotNull(SuccessDelays, "SuccessDelays != null");
            Assert.IsNotNull(SuccessInitialVectors, "SuccessInitialVectors != null");

            Assert.IsNotNull(Background, "Background != null");

            foreach (var letter in SuccessLetters!)
            {
                letter.gameObject.SetActive(value: false);
            }

            Background!.enabled = false;
            Background!.DOFade(endValue: 0f, duration: 0f);
        }

        internal UniTask PlaySuccess()
        {
            Background!.enabled = true;
            Background!.DOFade(endValue: 0.5f, duration: 0.2f);

            Text[] successLetters = SuccessLetters!;
            var tasks = new List<UniTask>();

            for (var letterIndex = 0; letterIndex < successLetters.Length; ++letterIndex)
            {
                var letter = successLetters[letterIndex];
                var duration = SuccessDurations![letterIndex];
                var delay = SuccessDelays![letterIndex];
                var initialVector = SuccessInitialVectors![letterIndex];

                tasks.Add(DoHoming(letter, duration, initialVector, delay));
            }

            return UniTask.WhenAll(tasks);
        }
    }
}
