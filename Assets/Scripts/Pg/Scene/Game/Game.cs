#nullable enable
using Cysharp.Threading.Tasks;
using Pg.App;
using Pg.Puzzle;
using Pg.Puzzle.Util;
using Pg.Scene.Game.Internal;
using Pg.SceneData;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    public class Game
        : MonoBehaviour
    {
        [SerializeField]
        Coordinates? Coordinates;

        [SerializeField]
        GameData? GameData;

        [SerializeField]
        StartDirection? StartDirection;

        [SerializeField]
        UserPlayer? UserPlayer;

        [SerializeField]
        Score? Score;

        [SerializeField]
        GameEndDirection? GameEndDirection;

        [SerializeField]
        NextScreen? NextScreen;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
            Assert.IsNotNull(GameData, "GameData != null");
            Assert.IsNotNull(StartDirection, "StartDirection != null");
            Assert.IsNotNull(UserPlayer, "UserPlayer != null");
            Assert.IsNotNull(Score, "Score != null");
            Assert.IsNotNull(GameEndDirection, "GameEndDirection != null");
            Assert.IsNotNull(NextScreen, "NextScreen != null");
        }

        async void Start()
        {
            await StartDirection!.Play();
            var gameController = new GameController(GameData!);
            var tileStatuses = gameController.StartGame();
            await Coordinates!.SetTileEvents(UserPlayer!);
            Coordinates!.ApplyTiles(tileStatuses);
            Score!.Initialize(Data.Response.Score.Zero);

            UserPlayer!.OnTransaction
                .Subscribe(async tileOperation =>
                {
                    var turnResponse = gameController.ProcessTurn(tileOperation);

                    foreach (var stepResponse in turnResponse.SimulationStepResponses)
                    {
                        Debug.Log(stepResponse);
                        Debug.Log(Dumper.Dump(gameController.DebugGetTileStatuses()));
                        Debug.Log($"NEW SCORE: {stepResponse.AcquisitionScore}");
                        Coordinates!.ApplyTiles(stepResponse.BeginningMap);
                        await Coordinates!.ApplyVanishings(stepResponse.VanishingClusters);
                        Score!.AddScore(stepResponse.AcquisitionScore);
                        await Coordinates!.ApplySlides(stepResponse.SlidingGems);
                        Coordinates!.ApplyTiles(gameController.DebugGetTileStatuses());
                    }

                    Score!.SetScore(turnResponse.Score);

                    Debug.Log($"JUDGE: {turnResponse.JudgeResult}");

                    await turnResponse.JudgeResult.Switch(
                        () => UniTask.CompletedTask,
                        PlayFailure,
                        PlaySucceed
                    );
                })
                .AddTo(gameObject);
        }

        async UniTask PlayFailure()
        {
            await GameEndDirection!.PlayFailure();
            await NextScreen!.BlockUntilTap();
            SceneManager.MoveToResultScene(new ResultData());
        }

        async UniTask PlaySucceed()
        {
            await GameEndDirection!.PlaySucceed();
            await NextScreen!.BlockUntilTap();
            SceneManager.MoveToResultScene(new ResultData());
        }
    }
}
