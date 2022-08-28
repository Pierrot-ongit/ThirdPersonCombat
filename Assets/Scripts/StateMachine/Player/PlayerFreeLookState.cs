using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {

        private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int FreeLookBlendTree= Animator.StringToHash("FreeLookBlendTree");
        private readonly bool shouldFade;
        private const float AnimatorDampTime = 0.1f;

        public PlayerFreeLookState(PlayerStateMachine newStateMachine, bool shouldFade = true) : base(newStateMachine)
        {
            this.shouldFade = shouldFade;
        }

        public override void Enter()
        {
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0);
            if (shouldFade)
            {
                stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree, AnimatorDampTime);
            }
            else
            {
                stateMachine.Animator.Play(FreeLookBlendTree);
            }
        }


        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackingSate(stateMachine, 0));
                return;
            }
            
            Vector3 movement = CalculateMovement();
            
            Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

            FaceMovementDirection(movement, deltaTime);
        }


        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.JumpEvent -= OnJump;
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            Vector3 right = stateMachine.MainCameraTransform.right;
            forward.y = right.y = 0;
            forward.Normalize();
            right.Normalize();

            return forward * stateMachine.InputReader.MovementValue.y +
                   right * stateMachine.InputReader.MovementValue.x;
        }
        
        private void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(
                stateMachine.transform.rotation,
                Quaternion.LookRotation(movement),
                deltaTime * stateMachine.RotationDamping
            );
        }
        
        private void OnTarget()
        {
            if (!stateMachine.Targeter.SelectTarget()) { return;}
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        
        private void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }
    }
}
