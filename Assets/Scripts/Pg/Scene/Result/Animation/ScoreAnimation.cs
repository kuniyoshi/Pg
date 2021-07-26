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

            var asyncEnumerable = UniTaskAsyncEnumerable.Create<float>(async (writer, token) =>
                {
                    var elapsed = 0f;

                    while (!token.IsCancellationRequested && elapsed < Duration)
                    {
                        await writer.YieldAsync(Mathf.Lerp(a: 0, Mathf.Log(toValue), elapsed / Duration));
                        elapsed = elapsed + Time.deltaTime;
                        await UniTask.Yield(token);
                        elapsed = elapsed + Time.deltaTime;
                        await UniTask.Yield(token);
                        elapsed = elapsed + Time.deltaTime;
                        await UniTask.Yield(token);
                        elapsed = elapsed + Time.deltaTime;
                        await UniTask.Yield(token);
                        elapsed = elapsed + Time.deltaTime;
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
