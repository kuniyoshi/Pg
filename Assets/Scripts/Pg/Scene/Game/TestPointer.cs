#nullable enable
using Pg.App.Util;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    public class TestPointer
        : MonoBehaviour
    {
        void Start()
        {
            var image = this.GetComponentStrictly<Image>();
            image.OnPointerDownAsObservable()
                .Subscribe(data => { Debug.Log("down"); })
                .AddTo(gameObject);
            image.OnPointerEnterAsObservable()
                .Subscribe(data => { Debug.Log("enter"); })
                .AddTo(gameObject);
            image.OnPointerClickAsObservable()
                .Subscribe(data => { Debug.Log("click"); })
                .AddTo(gameObject);
        }
    }
}
