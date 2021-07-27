#nullable enable
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Result.Animation
{
    internal class ScoreAnimation
        : MonoBehaviour
    {
        [SerializeField]
        Text? ScoreText;

        [SerializeField]
        float Duration;

        [SerializeField]
        int TargetFps;

        void Awake()
        {
            Assert.IsNotNull(ScoreText, "ScoreText != null");
        }

        void Start()
        {
            ScoreText!.text = "00000000";
        }

        internal async UniTask Play(int toValue)
        {
            ScoreText!.text = "0";
            var secondsPerFrame = 1f / TargetFps;

            var asyncEnumerable = UniTaskAsyncEnumerable.Create<float>(async (writer, token) =>
                {
                    var elapsed = 0f;

                    while (!token.IsCancellationRequested && elapsed < Duration)
                    {
                        await writer.YieldAsync(Mathf.Lerp(a: 0, Mathf.Log(toValue), elapsed / Duration));

                        var step = 0f;

                        while (!token.IsCancellationRequested && step + Time.deltaTime < secondsPerFrame)
                        {
                            await UniTask.Yield(token);
                            step = step + Time.deltaTime;
                        }

                        elapsed = elapsed + step;
                    }
                }
            );

            await foreach (var value in asyncEnumerable)
            {
                ScoreText!.text = ((int) Mathf.Exp(value)).ToString();
            }

            ScoreText!.text = toValue.ToString();
        }
    }
}
