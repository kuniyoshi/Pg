#nullable enable
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pg.Scene.Game
{
    public class MyInputProcessor
        : InputProcessor<Vector2>
    {
        public override Vector2 Process(Vector2 value, InputControl control)
        {
            throw new NotImplementedException();
        }
    }
}
