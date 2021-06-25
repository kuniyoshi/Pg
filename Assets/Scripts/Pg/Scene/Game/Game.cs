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
            GameController.StartGame(GameData!);
            await Coordinates!.SetTileEvents(UserPlayer!);
            Coordinates!.ApplyTiles(GameController.Tiles);

            UserPlayer!.OnTransaction
                .Subscribe(tileOperation =>
                {
                    GameController.WorkTransaction(tileOperation);
                    Coordinates!.ApplyTiles(GameController.Tiles);
                    var clusters = GameController.ProcessTurn();
                    Debug.Log(clusters);
                })
                .AddTo(gameObject);
        }
    }
}
