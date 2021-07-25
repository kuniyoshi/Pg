#nullable enable
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
        Image? Background;

        [SerializeField]
        Text? Text;

        void Awake()
        {
            Assert.IsNotNull(Background, "Background != null");
            Assert.IsNotNull(Text, "Text != null");
        }

        void Start()
        {
            Hide();
        }

        internal void Play(GameResult gameResult)
        {
            if (gameResult != GameResult.Failure)
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
        }

        void Hide()
        {
            Background!.enabled = false;
            Text!.enabled = false;
        }
    }
}
