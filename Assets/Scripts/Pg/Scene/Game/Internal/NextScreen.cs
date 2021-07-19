#nullable enable
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game.Internal
{
    public class NextScreen
        : MonoBehaviour
    {
        [SerializeField]
        Image? Background;

        [SerializeField]
        Button? Button;

        void Awake()
        {
            Assert.IsNotNull(Background, "Image != null");
            Assert.IsNotNull(Button, "Button != null");
            Background!.enabled = false;
        }

        internal UniTask BlockUntilTap()
        {
            Background!.enabled = true;
            return Button.OnClickAsync();
        }
    }
}
