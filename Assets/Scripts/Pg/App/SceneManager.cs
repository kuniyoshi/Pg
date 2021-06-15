#nullable enable
using Pg.Etc;

namespace Pg.App
{
    public static class SceneManager
    {
        public static void MoveToGameScene()
        {
            LoadScene(SceneType.GameScene);
        }

        static void LoadScene(SceneType sceneType)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneType.ToString());
        }

        public static void MoveToResultScene()
        {
            LoadScene(SceneType.ResultScene);
        }

        public static void MoveBackToTitleScene()
        {
            LoadScene(SceneType.TitleScene);
        }
    }
}
