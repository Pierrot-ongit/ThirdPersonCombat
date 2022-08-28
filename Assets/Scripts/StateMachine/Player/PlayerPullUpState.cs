using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerPullUpState : PlayerBaseState
    {
        private readonly int PullupHash = Animator.StringToHash("Pullup");
        private const float AnimatorDampTime = 0.1f;
        private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
        
        public PlayerPullUpState(PlayerStateMachine newStateMachine) : base(newStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(PullupHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f) return;

            stateMachine.Controller.enabled = false;
            stateMachine.transform.Translate(Offset, Space.Self);
            stateMachine.Controller.enabled = true;
            
            // The player position has been moved by the animation.
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
        }

        public override void Exit()
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
        }
    }
}