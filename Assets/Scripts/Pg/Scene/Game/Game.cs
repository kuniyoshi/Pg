#nullable enable
using Pg.Puzzle;
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

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
            Assert.IsNotNull(GameData, "GameData != null");
            Assert.IsNotNull(StartDirection, "StartDirection != null");
            Assert.IsNotNull(UserPlayer, "UserPlayer != null");
        }

        async void Start()
        {
            await StartDirection!.Play();
            var gameController = new GameController(GameData!);
            var tileStatuses = gameController.StartGame();
            await Coordinates!.SetTileEvents(UserPlayer!);
            Coordinates!.ApplyTiles(tileStatuses);

            UserPlayer!.OnTransaction
                .Subscribe(tileOperation =>
                {
                    var resultAfterOperation = gameController.WorkTransaction(tileOperation);
                    Coordinates!.ApplyTiles(resultAfterOperation);
                    var simulationStepData = gameController.ProcessTurn();
                    Debug.Log(simulationStepData);
                    Debug.Log(gameController.DebugGetTileStatuses());
                })
                .AddTo(gameObject);
        }
    }
}
