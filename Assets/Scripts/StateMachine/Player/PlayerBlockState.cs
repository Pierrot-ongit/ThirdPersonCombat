using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerBlockState : PlayerBaseState
    {
        public PlayerBlockState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }

        private readonly int BLockHash = Animator.StringToHash("BlockIdle");
        private const float AnimatorDampTime = 0.1f;
        public override void Enter()
        {
            stateMachine.Health.SetInvulnerable(true);
            stateMachine.Animator.CrossFadeInFixedTime(BLockHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (!stateMachine.InputReader.IsBlocking)
            {
                ReturnToLocomotion();
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetInvulnerable(false);
        }
        
    }
}
