// GENERATED AUTOMATICALLY FROM 'Assets/Input/CharacterController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CharacterController : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CharacterController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CharacterController"",
    ""maps"": [
        {
            ""name"": ""Mouse and Keyboard"",
            ""id"": ""4cca6537-0800-43b9-88b0-e591bb9889da"",
            ""actions"": [
                {
                    ""name"": ""Pick"",
                    ""type"": ""Button"",
                    ""id"": ""94de72ba-f381-4fda-a555-8d9475bb4c97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""4757960d-484b-4862-8c3d-b9beb632ddc0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b3b4cf93-14aa-4e2e-b694-7c06a8e8f8c0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""32bf1302-1af6-4130-9982-05a7c8028ddd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""75c70c0c-239f-4b21-9c52-75476bfca265"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""42a0cf33-3407-4cfa-a07d-1487d62e48fa"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""301b5425-34a3-45b1-9b60-f3ad4ac8665f"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""62d86b95-90de-4e89-87bf-f67f4aa40f60"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""57db117b-2310-41ab-a968-1b44c3e44f39"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""990cf6a5-a871-40b8-8e18-a2f0e5f84ede"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4668d4c7-e196-499c-a449-704b704c2fe5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""adfc414a-c237-4a8f-b551-eab5ee7774ab"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""025631a0-de13-411f-96c8-0c7d74e0359c"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f859a7b-3db9-4942-a83c-5a73da02ed13"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""id"": ""fdcacab4-1817-4b77-a448-e281abb78ce4"",
            ""actions"": [
                {
                    ""name"": ""Move_Controller"",
                    ""type"": ""Value"",
                    ""id"": ""d2f6859c-8449-4f6a-9c65-f49881bcfbc8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look_Controller"",
                    ""type"": ""Value"",
                    ""id"": ""bca7d8f4-a3d5-42ef-84a9-7b6824ea123a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pick_Controller"",
                    ""type"": ""Button"",
                    ""id"": ""b4097c96-0374-4638-991f-d0216673b073"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll_Controller"",
                    ""type"": ""Value"",
                    ""id"": ""f7d05f82-2d0c-4c1c-94bd-158bd86d9b42"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1ec4a793-88c7-4c92-a3dc-67d1db1cc7e8"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move_Controller"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a496f99-11c7-48ec-95e3-217112cac060"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look_Controller"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1e6f59f-e890-40a0-89fd-217045f29211"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pick_Controller"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse and Keyboard
        m_MouseandKeyboard = asset.FindActionMap("Mouse and Keyboard", throwIfNotFound: true);
        m_MouseandKeyboard_Pick = m_MouseandKeyboard.FindAction("Pick", throwIfNotFound: true);
        m_MouseandKeyboard_Scroll = m_MouseandKeyboard.FindAction("Scroll", throwIfNotFound: true);
        m_MouseandKeyboard_Move = m_MouseandKeyboard.FindAction("Move", throwIfNotFound: true);
        m_MouseandKeyboard_Look = m_MouseandKeyboard.FindAction("Look", throwIfNotFound: true);
        m_MouseandKeyboard_Pause = m_MouseandKeyboard.FindAction("Pause", throwIfNotFound: true);
        // Controller
        m_Controller = asset.FindActionMap("Controller", throwIfNotFound: true);
        m_Controller_Move_Controller = m_Controller.FindAction("Move_Controller", throwIfNotFound: true);
        m_Controller_Look_Controller = m_Controller.FindAction("Look_Controller", throwIfNotFound: true);
        m_Controller_Pick_Controller = m_Controller.FindAction("Pick_Controller", throwIfNotFound: true);
        m_Controller_Scroll_Controller = m_Controller.FindAction("Scroll_Controller", throwIfNotFound: true);
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

    // Mouse and Keyboard
    private readonly InputActionMap m_MouseandKeyboard;
    private IMouseandKeyboardActions m_MouseandKeyboardActionsCallbackInterface;
    private readonly InputAction m_MouseandKeyboard_Pick;
    private readonly InputAction m_MouseandKeyboard_Scroll;
    private readonly InputAction m_MouseandKeyboard_Move;
    private readonly InputAction m_MouseandKeyboard_Look;
    private readonly InputAction m_MouseandKeyboard_Pause;
    public struct MouseandKeyboardActions
    {
        private @CharacterController m_Wrapper;
        public MouseandKeyboardActions(@CharacterController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pick => m_Wrapper.m_MouseandKeyboard_Pick;
        public InputAction @Scroll => m_Wrapper.m_MouseandKeyboard_Scroll;
        public InputAction @Move => m_Wrapper.m_MouseandKeyboard_Move;
        public InputAction @Look => m_Wrapper.m_MouseandKeyboard_Look;
        public InputAction @Pause => m_Wrapper.m_MouseandKeyboard_Pause;
        public InputActionMap Get() { return m_Wrapper.m_MouseandKeyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseandKeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IMouseandKeyboardActions instance)
        {
            if (m_Wrapper.m_MouseandKeyboardActionsCallbackInterface != null)
            {
                @Pick.started -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPick;
                @Pick.performed -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPick;
                @Pick.canceled -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPick;
                @Scroll.started -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnScroll;
                @Move.started -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnLook;
                @Pause.started -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MouseandKeyboardActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MouseandKeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pick.started += instance.OnPick;
                @Pick.performed += instance.OnPick;
                @Pick.canceled += instance.OnPick;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MouseandKeyboardActions @MouseandKeyboard => new MouseandKeyboardActions(this);

    // Controller
    private readonly InputActionMap m_Controller;
    private IControllerActions m_ControllerActionsCallbackInterface;
    private readonly InputAction m_Controller_Move_Controller;
    private readonly InputAction m_Controller_Look_Controller;
    private readonly InputAction m_Controller_Pick_Controller;
    private readonly InputAction m_Controller_Scroll_Controller;
    public struct ControllerActions
    {
        private @CharacterController m_Wrapper;
        public ControllerActions(@CharacterController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move_Controller => m_Wrapper.m_Controller_Move_Controller;
        public InputAction @Look_Controller => m_Wrapper.m_Controller_Look_Controller;
        public InputAction @Pick_Controller => m_Wrapper.m_Controller_Pick_Controller;
        public InputAction @Scroll_Controller => m_Wrapper.m_Controller_Scroll_Controller;
        public InputActionMap Get() { return m_Wrapper.m_Controller; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControllerActions set) { return set.Get(); }
        public void SetCallbacks(IControllerActions instance)
        {
            if (m_Wrapper.m_ControllerActionsCallbackInterface != null)
            {
                @Move_Controller.started -= m_Wrapper.m_ControllerActionsCallbackInterface.OnMove_Controller;
                @Move_Controller.performed -= m_Wrapper.m_ControllerActionsCallbackInterface.OnMove_Controller;
                @Move_Controller.canceled -= m_Wrapper.m_ControllerActionsCallbackInterface.OnMove_Controller;
                @Look_Controller.started -= m_Wrapper.m_ControllerActionsCallbackInterface.OnLook_Controller;
                @Look_Controller.performed -= m_Wrapper.m_ControllerActionsCallbackInterface.OnLook_Controller;
                @Look_Controller.canceled -= m_Wrapper.m_ControllerActionsCallbackInterface.OnLook_Controller;
                @Pick_Controller.started -= m_Wrapper.m_ControllerActionsCallbackInterface.OnPick_Controller;
                @Pick_Controller.performed -= m_Wrapper.m_ControllerActionsCallbackInterface.OnPick_Controller;
                @Pick_Controller.canceled -= m_Wrapper.m_ControllerActionsCallbackInterface.OnPick_Controller;
                @Scroll_Controller.started -= m_Wrapper.m_ControllerActionsCallbackInterface.OnScroll_Controller;
                @Scroll_Controller.performed -= m_Wrapper.m_ControllerActionsCallbackInterface.OnScroll_Controller;
                @Scroll_Controller.canceled -= m_Wrapper.m_ControllerActionsCallbackInterface.OnScroll_Controller;
            }
            m_Wrapper.m_ControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move_Controller.started += instance.OnMove_Controller;
                @Move_Controller.performed += instance.OnMove_Controller;
                @Move_Controller.canceled += instance.OnMove_Controller;
                @Look_Controller.started += instance.OnLook_Controller;
                @Look_Controller.performed += instance.OnLook_Controller;
                @Look_Controller.canceled += instance.OnLook_Controller;
                @Pick_Controller.started += instance.OnPick_Controller;
                @Pick_Controller.performed += instance.OnPick_Controller;
                @Pick_Controller.canceled += instance.OnPick_Controller;
                @Scroll_Controller.started += instance.OnScroll_Controller;
                @Scroll_Controller.performed += instance.OnScroll_Controller;
                @Scroll_Controller.canceled += instance.OnScroll_Controller;
            }
        }
    }
    public ControllerActions @Controller => new ControllerActions(this);
    public interface IMouseandKeyboardActions
    {
        void OnPick(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IControllerActions
    {
        void OnMove_Controller(InputAction.CallbackContext context);
        void OnLook_Controller(InputAction.CallbackContext context);
        void OnPick_Controller(InputAction.CallbackContext context);
        void OnScroll_Controller(InputAction.CallbackContext context);
    }
}
