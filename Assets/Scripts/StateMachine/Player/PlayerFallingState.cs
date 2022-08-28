using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        public PlayerFallingState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }

        private readonly int FallHash = Animator.StringToHash("Fall");
        private const float AnimatorDampTime = 0.1f;
        private Vector3 momentum;
        public override void Enter()
        {
            momentum = stateMachine.Controller.velocity;
            momentum.y = 0;
            stateMachine.Animator.CrossFadeInFixedTime(FallHash, AnimatorDampTime);
            stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum, deltaTime);

            if (stateMachine.Controller.isGrounded)
            {
                ReturnToLocomotion();
            }
            FaceTarget();
        }

        public override void Exit()
        {
            stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        }
        
        private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
        {
            stateMachine.SwitchState(new PlayerHanging(stateMachine, ledgeForward, closestPoint));
        }
        
    }
}
