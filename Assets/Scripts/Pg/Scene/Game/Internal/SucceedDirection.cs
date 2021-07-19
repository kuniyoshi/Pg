#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game.Internal
{
    internal class SucceedDirection
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
        Text[]? SucceedLetters;

        [SerializeField]
        float[]? SucceedDurations;

        [SerializeField]
        float[]? SucceedDelays;

        [SerializeField]
        Vector3[]? SucceedInitialVectors;

        void Awake()
        {
            Assert.IsNotNull(SucceedLetters, "SucceedLetters != null");
            Assert.IsTrue(
                SucceedLetters!.Any() && SucceedLetters!.All(text => text != null),
                "SucceedLetters!.Any() && SucceedLetters!.All(text => text != null)"
            );

            Assert.IsNotNull(SucceedDurations, "SucceedDurations != null");
            Assert.IsNotNull(SucceedDelays, "SucceedDelays != null");
            Assert.IsNotNull(SucceedInitialVectors, "SucceedInitialVectors != null");

            foreach (var letter in SucceedLetters!)
            {
                letter.gameObject.SetActive(value: false);
            }
        }

        internal UniTask PlaySucceed()
        {
            Text[] succeedLetters = SucceedLetters!;
            var tasks = new List<UniTask>();

            for (var letterIndex = 0; letterIndex < succeedLetters.Length; ++letterIndex)
            {
                var letter = succeedLetters[letterIndex];
                var duration = SucceedDurations![letterIndex];
                var delay = SucceedDelays![letterIndex];
                var initialVector = SucceedInitialVectors![letterIndex];

                tasks.Add(DoHoming(letter, duration, initialVector, delay));
            }

            return UniTask.WhenAll(tasks);
        }
    }
}
