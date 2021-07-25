#nullable enable
using Pg.App;
using UnityEngine;

namespace Pg.Scene.Title
{
    internal class Title
        : MonoBehaviour
    {
        internal void MoveToGameScene()
        {
            SceneManager.MoveToGameScene();
        }
    }
}
