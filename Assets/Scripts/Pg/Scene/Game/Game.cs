#nullable enable
using System.Linq;
using Cysharp.Threading.Tasks;
using Pg.App;
using Pg.Puzzle;
using Pg.Puzzle.Util;
using Pg.Scene.Game.Direction;
using Pg.Scene.Game.Public;
using Pg.SceneData;
using Pg.SceneData.ResultItem;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    internal class Game
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

        Record? _record;

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
            _record = new Record(GameData!.Stage.MaxTurnCount, GameData!.Stage.TargetScore);

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

                        _record.IncreaseTurn();
                        var isInChain = false;

                        foreach (var stepResponse in turnResponse.SimulationStepResponses)
                        {
                            if (isInChain)
                            {
                                _record!.IncreaseChainCount();
                            }

                            isInChain = true;

                            // TODO: useless iteration ????, should be improved it.
                            _record!.AddVanishedGemCount(stepResponse.VanishingClusters.NewGemColorTypes.Sum(
                                    newGemColorType
                                        => stepResponse.VanishingClusters.GetVanishingCoordinatesOf(newGemColorType)
                                            .Sum(cluster => cluster.Count())
                                )
                            );

                            Debug.Log(stepResponse);
                            Debug.Log(Dumper.Dump(gameController.DebugGetTileStatuses()));
                            Debug.Log($"NEW SCORE: {stepResponse.AcquisitionScore}");
                            Coordinates!.ApplyTiles(stepResponse.BeginningMap);
                            await Coordinates!.ApplyVanishings(stepResponse.VanishingClusters);
                            Score!.AddScore(stepResponse.AcquisitionScore);
                            await Coordinates!.ApplySlides(stepResponse.SlidingGems);
                            Coordinates!.ApplyTiles(gameController.DebugGetTileStatuses());
                        }

                        _record.SetScore(turnResponse.Score);

                        Score!.SetScore(turnResponse.Score);

                        Debug.Log($"JUDGE: {turnResponse.JudgeResult}");

                        await turnResponse.JudgeResult.Switch(
                            () => UniTask.CompletedTask,
                            PlayFailure,
                            PlaySuccess
                        );
                    }
                )
                .AddTo(gameObject);
        }

        async UniTask PlayFailure()
        {
            await GameEndDirection!.PlayFailure();
            await NextScreen!.BlockUntilTap();
            SceneManager.MoveToResultScene(_record!.CreateFailure());
        }

        async UniTask PlaySuccess()
        {
            await GameEndDirection!.PlaySuccess();
            await NextScreen!.BlockUntilTap();
            SceneManager.MoveToResultScene(_record!.CreateSuccess());
        }

        class Record
        {
            int TargetScore { get; }
            int TurnLimit { get; }
            Data.Response.Score _lastScore;

            int _passedTurn;

            int _totalChainCount;

            int _totalVanishedGemCount;

            internal Record(int turnLimit, int targetScore)
            {
                TurnLimit = turnLimit;
                TargetScore = targetScore;
            }

            internal void AddVanishedGemCount(int amount)
            {
                _totalVanishedGemCount = _totalVanishedGemCount + amount;
            }

            internal ResultData CreateFailure()
            {
                return ResultData.Create(
                    GameResult.Failure,
                    _passedTurn,
                    TurnLimit,
                    _totalChainCount,
                    _totalVanishedGemCount,
                    _lastScore.GetValue(),
                    TargetScore
                );
            }

            internal ResultData CreateSuccess()
            {
                return ResultData.Create(
                    GameResult.Success,
                    _passedTurn,
                    TurnLimit,
                    _totalChainCount,
                    _totalVanishedGemCount,
                    _lastScore.GetValue(),
                    TargetScore
                );
            }

            internal void IncreaseChainCount()
            {
                _totalChainCount++;
            }

            internal void IncreaseTurn()
            {
                _passedTurn++;
            }

            internal void SetScore(Data.Response.Score newValue)
            {
                _lastScore = newValue;
            }
        }
    }
}
