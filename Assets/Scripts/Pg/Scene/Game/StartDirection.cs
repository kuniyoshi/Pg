#nullable enable
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal class StartDirection
        : MonoBehaviour
    {
        [SerializeField]
        Image? Cover;

        void Awake()
        {
            Assert.IsNotNull(Cover, "Cover != null");
        }

        internal async Task Play()
        {
            Cover!.raycastTarget = true;
            await Cover.DOFade(endValue: 0f, duration: 1f).AsyncWaitForStart();
            Cover!.raycastTarget = false;
        }
    }
}
