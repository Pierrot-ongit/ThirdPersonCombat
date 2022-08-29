using ThirdPersonCombat.Combat;
using ThirdPersonCombat.Combat.Enemy;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private AttackData currentAttack;
        private bool alreadyAppliedForce;
        
        public EnemyAttackingState(EnemyStateMachine newStateMachine, int AttackId) : base(newStateMachine)
        {
            currentAttack = stateMachine.Attacks[AttackId];
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
            
            
            stateMachine.SetLastAttackTime(Time.time); // TODO A CONSERVER ?
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
            // todo reactivate the agent.
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