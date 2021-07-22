#nullable enable
using Pg.SceneData;
using UnityEngine;

namespace Pg.Scene.Result
{
    public class Result
        : MonoBehaviour
    {
        public void MoveToTitle()
        {
            var data = TheSceneData.GetResultData();
            Debug.Log(data);
            // SceneManager.MoveBackToTitleScene();
        }
    }
}
