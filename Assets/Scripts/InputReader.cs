using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonCombat
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
        public event Action JumpEvent;
        public event Action DodgeEvent;
        public event Action TargetEvent;
        public event Action HeavyEvent;

        private Controls controls;
        public bool IsAttacking { get; private set; }
        public bool IsBlocking{ get; private set; }
        
        private void Start()
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        private void OnDestroy()
        {
            controls.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) { return;}
            JumpEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) { return;}
            DodgeEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
           
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) { return;}
            TargetEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
            
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) { return;}
            HeavyEvent?.Invoke();
        }

        public void OnBlocking(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsBlocking = true;
            }
            else if (context.canceled)
            {
                IsBlocking = false;
            }
        }
    }
}