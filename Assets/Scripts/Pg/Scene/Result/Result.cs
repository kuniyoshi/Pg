#nullable enable
using Pg.App;
using UnityEngine;

namespace Pg.Scene.Result
{
    public class Result
        : MonoBehaviour
    {
        public void MoveToTitle()
        {
            SceneManager.MoveBackToTitleScene();
        }
    }
}
