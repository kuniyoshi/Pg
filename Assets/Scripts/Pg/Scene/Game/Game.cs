#nullable enable
using Pg.Data;
using Pg.Puzzle;
using Pg.Puzzle.Util;
using Pg.Rule;
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
            Score!.Initialize(Data.Score.Zero);

            UserPlayer!.OnTransaction
                .Subscribe(async tileOperation =>
                {
                    var simulationStepDataList = gameController.ProcessTurn(tileOperation);
                    var currentTurn = gameController.PassedTurn;

                    var chainingCount = 0;

                    foreach (var simulationStepData in simulationStepDataList)
                    {
                        var score = CalculateScore.StepCalculate(
                            simulationStepData.VanishingClusters,
                            new ChainingCount(chainingCount++),
                            currentTurn
                        );
                        Debug.Log(simulationStepData);
                        Debug.Log(Dumper.Dump(gameController.DebugGetTileStatuses()));
                        Coordinates!.ApplyTiles(simulationStepData.BeginningMap);
                        await Coordinates!.ApplyVanishings(simulationStepData.VanishingClusters);
                        Score!.AddScore(score);
                        await Coordinates!.ApplySlides(simulationStepData.SlidingGems);
                        Coordinates!.ApplyTiles(gameController.DebugGetTileStatuses());
                    }
                })
                .AddTo(gameObject);
        }
    }
}
