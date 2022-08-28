using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private readonly int AttackHash = Animator.StringToHash("Attack");
        private const float AnimatorDampTime = 0.1f;
        
        public EnemyAttackingState(EnemyStateMachine newStateMachine) : base(newStateMachine)
        {
        }

        public override void Enter()
        {
            FacePlayer();
            stateMachine.Animator.CrossFadeInFixedTime(AttackHash, AnimatorDampTime);
            stateMachine.WeaponDamage.SetAttackDamage(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1f)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }

        }

        public override void Exit()
        {

        }
    }
}