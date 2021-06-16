#nullable enable
using System;
using Pg.App;
using Pg.Puzzle;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    public class Game
    : MonoBehaviour
    {
        float _startedAt;

        [SerializeField]
        float Duration;

        [SerializeField]
        Coordinates? Coordinates;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
        }


        void Start()
        {
            _startedAt = Time.time;
            GameController.StartGame();
        }

        void Update()
        {
            Coordinates.ApplyTiles(GameController.Tiles);
            if (Time.time > _startedAt + Duration)
            {
                SceneManager.MoveToResultScene();
            }
        }
    }
}
