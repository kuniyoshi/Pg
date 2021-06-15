#nullable enable
using Pg.App;
using UnityEngine;

namespace Pg.Scene.Title
{
    public class Start
        : MonoBehaviour
    {
        public void MoveToGameScene()
        {
            SceneManager.MoveToGameScene();
        }
    }
}
