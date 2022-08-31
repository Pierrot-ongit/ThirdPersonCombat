using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        public PlayerImpactState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }

        private readonly int ImpactHash = Animator.StringToHash("Impact");
        private const float AnimatorDampTime = 0.1f;
        private float duration = 1f;
        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Impact");
            if (normalizedTime >= 1f)
            {
                ReturnToLocomotion();
            }

            duration -= deltaTime;
            if (duration <= 0f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {

        }
        
    }
}
