using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerHanging : PlayerBaseState
    {
        private readonly int HangingHash = Animator.StringToHash("Hanging");
        private readonly Vector3 closestPoint;
        private readonly Vector3 ledgeForward;
        private const float AnimatorDampTime = 0.1f;
        
        public PlayerHanging(PlayerStateMachine newStateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(newStateMachine)
        {
            this.ledgeForward = ledgeForward;
            this.closestPoint = closestPoint;
        }
        
        public override void Enter()
        {
            stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
            stateMachine.Controller.enabled = false;
            stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
            stateMachine.Controller.enabled = true;
            stateMachine.Animator.CrossFadeInFixedTime(HangingHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputReader.MovementValue.y < 0f)
            {
                stateMachine.Controller.Move(Vector3.zero);
                stateMachine.ForceReceiver.Reset();
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            }
            else if (stateMachine.InputReader.MovementValue.y > 0f)
            {
                stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
            }
        }

        public override void Exit()
        {

        }
        
    }
}
