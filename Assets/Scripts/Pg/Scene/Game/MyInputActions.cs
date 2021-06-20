// GENERATED AUTOMATICALLY FROM 'Assets/Settings/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Pg.Scene.Game
{
    public class @MyInputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @MyInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""dd22c92f-cbc3-45a1-a72b-35b4d51e9a0c"",
            ""actions"": [
                {
                    ""name"": ""SelectTile"",
                    ""type"": ""Value"",
                    ""id"": ""6e318b99-3105-4273-93e3-b81c3c8756d8"",
                    ""expectedControlType"": ""Touch"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SwapTile"",
                    ""type"": ""Button"",
                    ""id"": ""6999200d-6882-4c9e-a66a-f8216d707508"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""CompleteTile"",
                    ""type"": ""Button"",
                    ""id"": ""ac1da77c-c443-438b-82f6-530d996a7c8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""65f298db-9998-4ad8-a6c5-8ac4b367dd37"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Main"",
                    ""action"": ""SelectTile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76ef7ec9-ed99-403b-b286-ae10f19a0286"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Main"",
                    ""action"": ""SwapTile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00a2d1c8-f766-4825-931f-2c50b49bc22f"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Main"",
                    ""action"": ""CompleteTile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Main"",
            ""bindingGroup"": ""Main"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_SelectTile = m_Player.FindAction("SelectTile", throwIfNotFound: true);
            m_Player_SwapTile = m_Player.FindAction("SwapTile", throwIfNotFound: true);
            m_Player_CompleteTile = m_Player.FindAction("CompleteTile", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_SelectTile;
        private readonly InputAction m_Player_SwapTile;
        private readonly InputAction m_Player_CompleteTile;
        public struct PlayerActions
        {
            private @MyInputActions m_Wrapper;
            public PlayerActions(@MyInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @SelectTile => m_Wrapper.m_Player_SelectTile;
            public InputAction @SwapTile => m_Wrapper.m_Player_SwapTile;
            public InputAction @CompleteTile => m_Wrapper.m_Player_CompleteTile;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @SelectTile.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectTile;
                    @SelectTile.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectTile;
                    @SelectTile.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectTile;
                    @SwapTile.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapTile;
                    @SwapTile.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapTile;
                    @SwapTile.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwapTile;
                    @CompleteTile.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCompleteTile;
                    @CompleteTile.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCompleteTile;
                    @CompleteTile.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCompleteTile;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SelectTile.started += instance.OnSelectTile;
                    @SelectTile.performed += instance.OnSelectTile;
                    @SelectTile.canceled += instance.OnSelectTile;
                    @SwapTile.started += instance.OnSwapTile;
                    @SwapTile.performed += instance.OnSwapTile;
                    @SwapTile.canceled += instance.OnSwapTile;
                    @CompleteTile.started += instance.OnCompleteTile;
                    @CompleteTile.performed += instance.OnCompleteTile;
                    @CompleteTile.canceled += instance.OnCompleteTile;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        private int m_MainSchemeIndex = -1;
        public InputControlScheme MainScheme
        {
            get
            {
                if (m_MainSchemeIndex == -1) m_MainSchemeIndex = asset.FindControlSchemeIndex("Main");
                return asset.controlSchemes[m_MainSchemeIndex];
            }
        }
        public interface IPlayerActions
        {
            void OnSelectTile(InputAction.CallbackContext context);
            void OnSwapTile(InputAction.CallbackContext context);
            void OnCompleteTile(InputAction.CallbackContext context);
        }
    }
}
