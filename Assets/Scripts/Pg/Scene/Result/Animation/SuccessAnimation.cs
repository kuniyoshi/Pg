#nullable enable
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
        Vector3 JumpEndValue;

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

        void Start()
        {
            Hide();
        }

        public void Play(GameResult gameResult)
        {
            if (gameResult != GameResult.Success)
            {
                Hide();

                return;
            }

            Appear();
        }

        void Appear()
        {
            Text!.enabled = true;
            Background!.enabled = true;

            Text!.rectTransform.DOJump(Vector3.zero, JumpPower, JumpCount, JumpDuration)
                .SetRelative();
            Background!.rectTransform.DOMoveX(-BackgroundX, duration: 0f)
                .SetRelative();
            Background!.rectTransform.DOMoveX(BackgroundX, BackgroundDuration)
                .SetRelative();
        }

        void Hide()
        {
            Background!.enabled = false;
            Text!.enabled = false;
        }
    }
}
