#nullable enable
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pg.SceneData.ResultItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Result.Animation
{
    internal class SuccessAnimation
        : MonoBehaviour
    {
        [SerializeField]
        Image? Background;

        [SerializeField]
        Text? Text;

        [Header("Animation Setting")]
        [SerializeField]
        float JumpPower;

        [SerializeField]
        int JumpCount;

        [SerializeField]
        float JumpDuration;

        [SerializeField]
        float BackgroundX;

        [SerializeField]
        float BackgroundDuration;

        void Awake()
        {
            Assert.IsNotNull(Background, "Background != null");
            Assert.IsNotNull(Text, "Text != null");
        }

        async void Start()
        {
            await Hide();
        }

        internal async UniTask Play(GameResult gameResult)
        {
            if (gameResult != GameResult.Success)
            {
                await Hide();

                return;
            }

            await Appear();
        }

        async UniTask Appear()
        {
            Text!.enabled = true;
            Background!.enabled = true;

            await UniTask.WhenAll(
                Text!.rectTransform.DOJump(Vector3.zero, JumpPower, JumpCount, JumpDuration)
                    .SetRelative()
                    .ToUniTask(),
                UniTask.WhenAll(
                    Background!.rectTransform.DOMoveX(-BackgroundX, duration: 0f)
                        .SetRelative()
                        .ToUniTask(),
                    Background!.rectTransform.DOMoveX(BackgroundX, BackgroundDuration)
                        .SetRelative()
                        .ToUniTask()
                )
            );
        }

        async UniTask Hide()
        {
            Background!.enabled = false;
            Text!.enabled = false;
            await UniTask.CompletedTask;
        }
    }
}
