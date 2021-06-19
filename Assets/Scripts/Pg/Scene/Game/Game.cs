#nullable enable
using Pg.Puzzle;
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

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
            Assert.IsNotNull(GameData, "GameData != null");
        }

        void Start()
        {
            GameController.StartGame(GameData!);
        }

        void Update()
        {
            Coordinates!.ApplyTiles(GameController.Tiles);
        }
    }
}
