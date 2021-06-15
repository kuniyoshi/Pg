#nullable enable
using System;
using Pg.App;
using UnityEngine;

namespace Pg.Scene.Game
{
    public class Game
    : MonoBehaviour
    {
        float _startedAt;

        [SerializeField]
        float Duration;



        void Start()
        {
            _startedAt = Time.time;
        }

        void Update()
        {
            if (Time.time > _startedAt + Duration)
            {
                SceneManager.MoveToResultScene();
            }
        }
    }
}
