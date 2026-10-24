#nullable enable
using System.Linq;
using Cysharp.Threading.Tasks;
using Pg.App;
using Pg.Puzzle;
using Pg.Scene.Game.Direction;
using Pg.Scene.Game.Public;
using Pg.SceneData;
using Pg.SceneData.ResultItem;
using R3;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    internal class Game
        : MonoBehaviour
    {
        [SerializeField]
        Coordinates Coordinates = null!;

        [SerializeField]
        GameData GameData = null!;

        [SerializeField]
        StartDirection StartDirection = null!;

        [SerializeField]
        UserPlayer UserPlayer = null!;

        [SerializeField]
        Score Score = null!;

        [SerializeField]
        GameEndDirection GameEndDirection = null!;

        [SerializeField]
        NextScreen NextScreen = null!;

        Record? _record;
        GameController? _gameController;
        bool _findingHint;
        string _hintMessage = "";

        void OnGUI()
        {
            var enabled = GUI.enabled;
            GUI.enabled = _gameController != null && UserPlayer.CanRequestHint && !_findingHint;
            if (GUI.Button(new Rect(16, 64, 140, 36), _findingHint ? "Searching..." : "Hint"))
                FindHint();
            GUI.enabled = enabled;
            if (_hintMessage.Length > 0) GUI.Label(new Rect(16, 108, 400, 28), _hintMessage);
        }

        async void FindHint()
        {
            _findingHint = true;
            _hintMessage = "";
            UserPlayer.SetInputEnabled(false);
            Coordinates.ClearSelections();
            Coordinates.ClearHint();
            try
            {
                var hint = await _gameController!.FindHintAsync();
                if (this == null) return;
                if (hint == null) _hintMessage = "No hint available.";
                else Coordinates.ShowHint(hint);
            }
            catch (System.Exception exception)
            {
                if (this == null) return;
                _hintMessage = "Hint search failed.";
                Debug.LogException(exception);
            }
            finally
            {
                if (this != null)
                {
                    _findingHint = false;
                    UserPlayer.SetInputEnabled(true);
                }
            }
        }

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
            _record = new Record(GameData.Stage.MaxTurnCount, GameData.Stage.TargetScore);

            await StartDirection.Play();
            var random = new System.Random();
            var colors = Data.Simulation.GemColorType.Values.ToArray();
            var replenishmentGems = Enumerable.Range(0, GameController.CalculateRequiredGemCount(GameData.Stage))
                .Select(_ => colors[random.Next(colors.Length)])
                .ToArray();
            var gameController = new GameController(GameData, replenishmentGems);
            _gameController = gameController;
            var tileStatuses = gameController.StartGame();
            Coordinates.Initialize(tileStatuses, UserPlayer);
            Score.Initialize(Data.Response.Score.Zero);

            UserPlayer.OnTransaction
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

                            _record!.AddVanishedGemCount(stepResponse.VanishingClusters.WholeCoordinates.Distinct().Count());
                            Coordinates.AssertMatches(stepResponse.BeginningMap);
                            await Coordinates.ApplyVanishings(stepResponse.VanishingClusters);
                            Score.AddScore(stepResponse.AcquisitionScore);
                            await Coordinates.ApplySlides(stepResponse.SlidingGems);
                        }

                        Coordinates.AssertMatches(gameController.DebugGetTileStatuses());
                        _record.SetScore(turnResponse.Score);

                        Score.SetScore(turnResponse.Score);

                        Debug.Log($"JUDGE: {turnResponse.JudgeResult}");

                        await turnResponse.JudgeResult.Switch(
                            () =>
                            {
                                UserPlayer.SetInputEnabled(true);
                                return UniTask.CompletedTask;
                            },
                            PlayFailure,
                            PlaySuccess
                        );
                    }
                )
                .AddTo(gameObject);
            UserPlayer.SetInputEnabled(true);
        }

        async UniTask PlayFailure()
        {
            await GameEndDirection.PlayFailure();
            await NextScreen.BlockUntilTap();
            SceneManager.MoveToResultScene(_record!.CreateFailure());
        }

        async UniTask PlaySuccess()
        {
            await GameEndDirection.PlaySuccess();
            await NextScreen.BlockUntilTap();
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
