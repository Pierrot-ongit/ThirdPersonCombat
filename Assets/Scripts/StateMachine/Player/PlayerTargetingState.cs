using System.Numerics;
using Mono.Cecil.Cil;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        public PlayerTargetingState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }

        private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
        private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
        private readonly int TargetingRightHash= Animator.StringToHash("TargetingRight");
        private const float AnimatorDampTime = 0.1f;
        
        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree, AnimatorDampTime);
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.JumpEvent += OnJump;
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackingSate(stateMachine, 0));
                return;
            }
            
            if (stateMachine.InputReader.IsBlocking)
            {
                stateMachine.SwitchState(new PlayerBlockState(stateMachine));
                return;
            }
            
            if (stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            Vector3 movement = CalculateMovement(deltaTime);
            Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
            UpdateAnimator(deltaTime);
            FaceTarget();
        }

        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.JumpEvent -= OnJump;
        }
        
        private void OnTarget()
        {
            stateMachine.Targeter.CancelTargeting();
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        
        private Vector3 CalculateMovement(float deltaTime)
        {
            Vector3 movement = new Vector3();
            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
            
            return movement;
        }
        
        private void UpdateAnimator(float deltaTime)
        {
            if (stateMachine.InputReader.MovementValue.y == 0)
            {
                stateMachine.Animator.SetFloat(TargetingForwardHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(TargetingForwardHash, value, AnimatorDampTime, deltaTime);
            }
            
            if (stateMachine.InputReader.MovementValue.x == 0)
            {
                stateMachine.Animator.SetFloat(TargetingRightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(TargetingRightHash, value, AnimatorDampTime, deltaTime);
            }
        }

        private void OnDodge()
        {
            if (stateMachine.InputReader.MovementValue == Vector2.zero) { return;}
            stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
        }
        
        private void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }
    }
}
