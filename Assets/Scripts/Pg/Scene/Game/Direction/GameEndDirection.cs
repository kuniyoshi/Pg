#nullable enable
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game.Direction
{
    internal class GameEndDirection
        : MonoBehaviour
    {
        [SerializeField]
        FailureDirection FailureDirection = null!;

        [SerializeField]
        SuccessDirection SuccessDirection = null!;

        void Awake()
        {
            Assert.IsNotNull(FailureDirection, "FailureDirection != null");
            Assert.IsNotNull(SuccessDirection, "SuccessDirection != null");
        }

        internal UniTask PlayFailure()
        {
            return FailureDirection.PlayFailure();
        }

        internal UniTask PlaySuccess()
        {
            return SuccessDirection.PlaySuccess();
        }
    }
}
