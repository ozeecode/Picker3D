using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Picker3D
{
    public class InputManager : MonoBehaviour
    {
        private PlayerControls playerControls;
        [SerializeField] InputData inputData;
        [SerializeField, Range(0, .1f)] float swipeSensitivity = .05f;


        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Game.Enable();

            //for the development
            playerControls.Game.Horizontal.performed += Horizontal_performed;
            playerControls.Game.Horizontal.canceled += Horizontal_canceled;

            //Touch events
            playerControls.Game.TouchState.started += OnFirstTouch;
            playerControls.Game.TouchState.canceled += OnTouchEnd;
            playerControls.Game.Touch.performed += OnTouching;
        }
        private void OnTouching(InputAction.CallbackContext ctx)
        {
            TouchState touch = ctx.ReadValue<TouchState>();

            if (touch.phase == TouchPhase.Ended)
            {
                inputData.Horizontal = 0;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                inputData.Horizontal = touch.delta.x * swipeSensitivity;
            }
        }

        private void OnFirstTouch(InputAction.CallbackContext ctx)
        {
            inputData.TouchStart?.Invoke();
        }

        private void OnTouchEnd(InputAction.CallbackContext obj)
        {
            inputData.Horizontal = 0;
        }

        private void Horizontal_performed(InputAction.CallbackContext ctx)
        {
            inputData.Horizontal = ctx.ReadValue<float>();
        }
        private void Horizontal_canceled(InputAction.CallbackContext ctx)
        {
            inputData.Horizontal = 0;
        }

    }
}