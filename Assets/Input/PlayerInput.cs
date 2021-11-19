// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Character Controls"",
            ""id"": ""8e36a6c5-7491-4a36-8570-8548ccb7f646"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d843365a-8bed-40c7-b4cc-7984ef0be4e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""0f86ad33-566a-4460-88e8-5c75358e7e22"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5399bcad-325f-4698-a8b4-806dea877f44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UsePaw"",
                    ""type"": ""Button"",
                    ""id"": ""72e4e537-3038-4d07-bba5-f718beee1a4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""UseMouth"",
                    ""type"": ""Button"",
                    ""id"": ""7a280b4f-0fa9-41ee-8b80-fabceff90fb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""UseBody"",
                    ""type"": ""Button"",
                    ""id"": ""bef500e9-3595-4193-89e0-bbda1ab8ce88"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""677ca15d-7b3e-4a49-b771-b1aefd01bf31"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fe08f0ff-5b9b-4afc-bc22-e6a02681cc34"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2487855d-3dc7-497b-859b-cf0127654791"",
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
                    ""id"": ""d94f3755-2ff1-4fbb-99b9-c503c05ced4d"",
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
                    ""id"": ""b1bb71b5-8c3f-4783-b234-c1f7a1815332"",
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
                    ""id"": ""1620bd7a-bbb0-43d2-8e0b-39561661ce16"",
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
                    ""id"": ""699fa601-4569-4dc6-8238-341f42460144"",
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
                    ""id"": ""91d9377e-8d09-4a02-92ae-a4740b4d644f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e56b315e-699a-4d96-829a-8a82b4f61731"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d66a0ef9-1d6d-4ec9-a2a6-e41da91e3e65"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca96d85e-9dd2-4544-bd05-43e5ab979c76"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""773a9a44-188c-4617-b786-10b6f5e95690"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b89dc92a-7d90-4e8b-bbe4-bc78c809ae53"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePaw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""634f2cff-d530-468e-beba-542da27f28e8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseMouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67240661-d075-4b2b-b7b0-6595c04d3b75"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseMouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6d10a07-2c92-4618-a6de-dfb4b81e73bd"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseBody"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""768c5dd8-5321-4938-a7b9-72bd3f9c466e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseBody"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24586a83-f44f-42a8-b047-d80cc1e493b6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""2dab8fe6-7706-4b67-a6b5-1ead9f78e142"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""7f01030a-a7c5-4cea-89f9-bb519cf87327"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d3c4667d-7658-4b1a-a7d5-7237dc40a8f7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character Controls
        m_CharacterControls = asset.FindActionMap("Character Controls", throwIfNotFound: true);
        m_CharacterControls_Move = m_CharacterControls.FindAction("Move", throwIfNotFound: true);
        m_CharacterControls_Run = m_CharacterControls.FindAction("Run", throwIfNotFound: true);
        m_CharacterControls_Jump = m_CharacterControls.FindAction("Jump", throwIfNotFound: true);
        m_CharacterControls_UsePaw = m_CharacterControls.FindAction("UsePaw", throwIfNotFound: true);
        m_CharacterControls_UseMouth = m_CharacterControls.FindAction("UseMouth", throwIfNotFound: true);
        m_CharacterControls_UseBody = m_CharacterControls.FindAction("UseBody", throwIfNotFound: true);
        m_CharacterControls_Look = m_CharacterControls.FindAction("Look", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
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

    // Character Controls
    private readonly InputActionMap m_CharacterControls;
    private ICharacterControlsActions m_CharacterControlsActionsCallbackInterface;
    private readonly InputAction m_CharacterControls_Move;
    private readonly InputAction m_CharacterControls_Run;
    private readonly InputAction m_CharacterControls_Jump;
    private readonly InputAction m_CharacterControls_UsePaw;
    private readonly InputAction m_CharacterControls_UseMouth;
    private readonly InputAction m_CharacterControls_UseBody;
    private readonly InputAction m_CharacterControls_Look;
    public struct CharacterControlsActions
    {
        private @PlayerInput m_Wrapper;
        public CharacterControlsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CharacterControls_Move;
        public InputAction @Run => m_Wrapper.m_CharacterControls_Run;
        public InputAction @Jump => m_Wrapper.m_CharacterControls_Jump;
        public InputAction @UsePaw => m_Wrapper.m_CharacterControls_UsePaw;
        public InputAction @UseMouth => m_Wrapper.m_CharacterControls_UseMouth;
        public InputAction @UseBody => m_Wrapper.m_CharacterControls_UseBody;
        public InputAction @Look => m_Wrapper.m_CharacterControls_Look;
        public InputActionMap Get() { return m_Wrapper.m_CharacterControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterControlsActions instance)
        {
            if (m_Wrapper.m_CharacterControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Run.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnRun;
                @Jump.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @UsePaw.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUsePaw;
                @UsePaw.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUsePaw;
                @UsePaw.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUsePaw;
                @UseMouth.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseMouth;
                @UseMouth.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseMouth;
                @UseMouth.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseMouth;
                @UseBody.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseBody;
                @UseBody.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseBody;
                @UseBody.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnUseBody;
                @Look.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_CharacterControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @UsePaw.started += instance.OnUsePaw;
                @UsePaw.performed += instance.OnUsePaw;
                @UsePaw.canceled += instance.OnUsePaw;
                @UseMouth.started += instance.OnUseMouth;
                @UseMouth.performed += instance.OnUseMouth;
                @UseMouth.canceled += instance.OnUseMouth;
                @UseBody.started += instance.OnUseBody;
                @UseBody.performed += instance.OnUseBody;
                @UseBody.canceled += instance.OnUseBody;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public CharacterControlsActions @CharacterControls => new CharacterControlsActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Click;
    public struct UIActions
    {
        private @PlayerInput m_Wrapper;
        public UIActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface ICharacterControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnUsePaw(InputAction.CallbackContext context);
        void OnUseMouth(InputAction.CallbackContext context);
        void OnUseBody(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
}
