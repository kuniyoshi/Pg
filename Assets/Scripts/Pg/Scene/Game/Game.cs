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
            var tileStatuses = GameController.StartGame(GameData!);
            await Coordinates!.SetTileEvents(UserPlayer!);
            Coordinates!.ApplyTiles(tileStatuses);

            UserPlayer!.OnTransaction
                .Subscribe(tileOperation =>
                {
                    var resultAfterOperation = GameController.WorkTransaction(tileOperation);
                    Coordinates!.ApplyTiles(resultAfterOperation);
                    var clusters = GameController.ProcessTurn();
                    Debug.Log(clusters);
                })
                .AddTo(gameObject);
        }
    }
}
