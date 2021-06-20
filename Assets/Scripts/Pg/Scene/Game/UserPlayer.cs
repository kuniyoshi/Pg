#nullable enable
using System;
using Pg.App.Util;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Pg.Scene.Game
{
    public class UserPlayer
        : MonoBehaviour
    {

        InputAction? _selectTile;
        InputAction? _swapTile;
        InputAction? _completeTile;

        void Awake()
        {
            // DebugBindLogOnActionChange();
            var myInputActions = new MyInputActions();
            myInputActions.Enable();
            var player = myInputActions.Player;
            player.Enable();
             _selectTile = player.SelectTile;
             _swapTile = player.SwapTile;
             _completeTile = player.CompleteTile;
        }

        static void DebugBindLogOnActionChange()
        {
            InputSystem.onActionChange +=
                (obj, change) =>
                {
                    var inputAction = obj as InputAction;

                    if (inputAction == null)
                    {
                        return;
                    }

                    if (inputAction.name == "Point" && change == InputActionChange.ActionPerformed)
                    {
                        return;
                    }

                    if (inputAction.name == "ScrollWheel" && change == InputActionChange.ActionPerformed)
                    {
                        return;
                    }

                    Debug.Log($"{((InputAction) obj).name} {change}");
                    switch (change)
                    {
                        case InputActionChange.ActionStarted:
                        case InputActionChange.ActionPerformed:
                        case InputActionChange.ActionCanceled:
                            break;

                        case InputActionChange.ActionEnabled:
                            break;

                        case InputActionChange.ActionDisabled:
                            break;

                        case InputActionChange.ActionMapEnabled:
                            break;

                        case InputActionChange.ActionMapDisabled:
                            break;

                        case InputActionChange.BoundControlsAboutToChange:
                            break;

                        case InputActionChange.BoundControlsChanged:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(change), change, null);
                    }
                };
        }

        void Update()
        {
            if (_selectTile!.triggered)
            {
                Debug.Log($"--- select tile triggered");
            }
            if (_swapTile!.triggered)
            {
                Debug.Log($"--- swap tile triggered");
            }
            if (_completeTile!.triggered)
            {
                Debug.Log($"--- complete tile triggered");
            }
        }

        public void OnA()
        {
            Debug.Log("### event A");
        }
        public void OnB()
        {
            Debug.Log("### event B");
        }

        public void OnC()
        {
            Debug.Log("### event C");
        }
    }
}
