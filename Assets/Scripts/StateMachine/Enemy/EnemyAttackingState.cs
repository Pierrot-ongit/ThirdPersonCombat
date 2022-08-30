using ThirdPersonCombat.Combat;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private AttackData currentAttack;
        private bool alreadyAppliedForce;
        
        public EnemyAttackingState(EnemyStateMachine newStateMachine, AttackData attack) : base(newStateMachine)
        {
            currentAttack = attack;
            // todo desactivate the agent if the attack apply force.
        }

        public override void Enter()
        {
            FacePlayer();
            stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransistionDuration);

            if (stateMachine.AttackHandler != null)
            {
                stateMachine.AttackHandler.SetCurrentAttack(currentAttack);
                stateMachine.AttackHandler.SetTarget(stateMachine.Player);
            }

            stateMachine.SetLastAttackTime(currentAttack, Time.time);
        }

        public override void Tick(float deltaTime)
        {
            if (GetNormalizedTime(stateMachine.Animator, currentAttack.AnimationName) >= 1f)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.SetNextAttack(null);
            /*
            if (stateMachine.AttackHandler != null)
            {
                stateMachine.AttackHandler.SetCurrentAttack(null);
                stateMachine.AttackHandler.SetTarget(null);
            }
            */
        }

    }
}