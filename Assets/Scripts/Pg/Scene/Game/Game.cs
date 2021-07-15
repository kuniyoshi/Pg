#nullable enable
using Pg.Puzzle;
using Pg.Puzzle.Util;
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

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
            Assert.IsNotNull(GameData, "GameData != null");
            Assert.IsNotNull(StartDirection, "StartDirection != null");
            Assert.IsNotNull(UserPlayer, "UserPlayer != null");
            Assert.IsNotNull(Score, "Score != null");
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
                })
                .AddTo(gameObject);
        }
    }
}
