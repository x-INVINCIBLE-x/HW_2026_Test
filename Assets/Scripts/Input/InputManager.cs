using UnityEngine;
using UnityEngine.InputSystem;

namespace Doofus.Input
{
    // Manages player input and exposes the current movement input.
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public Vector2 MoveInput { get; private set; }

        private PlayerControls controls;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Player.Enable();
            controls.Player.Move.performed += OnMovePerformed;
            controls.Player.Move.canceled += OnMoveCanceled;
        }

        private void OnDisable()
        {
            controls.Player.Move.performed -= OnMovePerformed;
            controls.Player.Move.canceled -= OnMoveCanceled;
            controls.Player.Disable();
        }

        // Updates the movement input when the Move action is performed.
        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        // Resets the movement input when the Move action is canceled.
        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            MoveInput = Vector2.zero;
        }
    }
}