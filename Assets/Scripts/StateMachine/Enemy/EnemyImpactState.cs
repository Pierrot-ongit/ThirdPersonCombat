using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyImpactState : EnemyBaseState
    {
        public EnemyImpactState(EnemyStateMachine newStateMachine) : base(newStateMachine) { }

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
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
            
            duration -= deltaTime;
            if (duration <= 0f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }

        public override void Exit()
        {

        }
    }
}