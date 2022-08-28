using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        public PlayerJumpingState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }

        private readonly int JumpHash = Animator.StringToHash("Jump");
        private const float AnimatorDampTime = 0.1f;
        private Vector3 momentum;

        public override void Enter()
        {
            stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);
            momentum = stateMachine.Controller.velocity;
            momentum.y = 0;
            stateMachine.Animator.CrossFadeInFixedTime(JumpHash, AnimatorDampTime);
            stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum, deltaTime);
            // We are no longer moving up, but downward.
            if (stateMachine.Controller.velocity.y <= 0)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
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
