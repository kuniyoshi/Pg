#nullable enable
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pg.SceneData.ResultItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Result.Animation
{
    internal class FailureAnimation
        : MonoBehaviour
    {
        [SerializeField]
        Image? BackgroundLeft;

        [SerializeField]
        Image? BackgroundRight;

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

        [SerializeField]
        float FadeDuration;

        void Awake()
        {
            Assert.IsNotNull(BackgroundLeft, "BackgroundLeft != null");
            Assert.IsNotNull(BackgroundRight, "BackgroundRight != null");
            Assert.IsNotNull(Text, "Text != null");
        }

        async void Start()
        {
            await Hide();
        }

        internal async UniTask Play(GameResult gameResult)
        {
            if (gameResult != GameResult.Failure)
            {
                await Hide();

                return;
            }

            await Appear();
        }

        async UniTask Appear()
        {
            Text!.enabled = true;
            BackgroundLeft!.enabled = true;
            BackgroundRight!.enabled = true;

            BackgroundLeft!.rectTransform.DOMoveX(-BackgroundX, duration: 0f)
                .SetRelative();
            BackgroundRight!.rectTransform.DOMoveX(BackgroundX, duration: 0f)
                .SetRelative();

            await (
                Text!.rectTransform.DOJump(Vector3.zero, JumpPower, JumpCount, JumpDuration)
                    .SetRelative()
                    .ToUniTask(),
                BackgroundLeft!.rectTransform.DOMoveX(BackgroundX, BackgroundDuration)
                    .SetRelative()
                    .ToUniTask(),
                BackgroundRight!.rectTransform.DOMoveX(-BackgroundX, BackgroundDuration)
                    .SetRelative()
                    .ToUniTask()
            );

            await (
                BackgroundLeft!.DOFade(endValue: 0f, FadeDuration)
                    .ToUniTask(),
                BackgroundRight!.DOFade(endValue: 0f, FadeDuration)
                    .ToUniTask()
            );
        }

        async UniTask Hide()
        {
            BackgroundLeft!.enabled = false;
            BackgroundRight!.enabled = false;
            Text!.enabled = false;
            await UniTask.CompletedTask;
        }
    }
}
