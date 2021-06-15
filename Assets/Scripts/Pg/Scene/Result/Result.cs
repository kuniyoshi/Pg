#nullable enable
using System;
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
