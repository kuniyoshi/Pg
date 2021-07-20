#nullable enable
using Pg.SceneData;

namespace Pg.App
{
    public static class SceneManager
    {
        public static void MoveBackToTitleScene()
        {
            LoadScene(SceneType.TitleScene);
        }

        public static void MoveToGameScene()
        {
            LoadScene(SceneType.GameScene);
        }

        public static void MoveToResultScene(ResultData resultData)
        {
            TheSceneData.SetResultData(resultData);
            LoadScene(SceneType.ResultScene);
        }

        static void LoadScene(SceneType sceneType)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneType.ToString());
        }
    }
}
