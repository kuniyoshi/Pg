#nullable enable
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game.Internal
{
    internal class GameEndDirection
        : MonoBehaviour
    {
        [SerializeField]
        FailureDirection? FailureDirection;

        [SerializeField]
        SucceedDirection? SucceedDirection;

        void Awake()
        {
            Assert.IsNotNull(FailureDirection, "FailureDirection != null");
            Assert.IsNotNull(SucceedDirection, "SucceedDirection != null");
        }

        internal UniTask PlayFailure()
        {
            return FailureDirection!.PlayFailure();
        }

        internal UniTask PlaySucceed()
        {
            return SucceedDirection!.PlaySucceed();
        }
    }
}
