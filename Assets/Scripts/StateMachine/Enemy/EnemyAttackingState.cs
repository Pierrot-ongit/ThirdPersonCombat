using ThirdPersonCombat.Combat;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        private const float MinimalForceDistance = 1.5f;
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

            stateMachine.SetLastAttackTime(currentAttack, Time.time, currentAttack.IsHeavy);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if (normalizedTime >= 1f)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }
            else
            {
                if (currentAttack.Force > 0 && normalizedTime >= currentAttack.ForceTime)
                {
                    TryApplyForce();
                }
            }
        }

        public override void Exit()
        {
            stateMachine.SetNextAttack(null);
            stateMachine.AttackTokenSubscriber.ReleaseToken();
            /*
            if (stateMachine.AttackHandler != null)
            {
                stateMachine.AttackHandler.SetCurrentAttack(null);
                stateMachine.AttackHandler.SetTarget(null);
            }
            */
        }

        public float GetAttackRange()
        {
            return currentAttack.Range;
        }
        
        private void TryApplyForce()
        {
            if (alreadyAppliedForce) return;
            
            Vector3 targetPosition = stateMachine.transform.InverseTransformPoint(stateMachine.Player.transform.position);

            // Is target in front of us ?
            float distanceWithTarget = Vector3.Dot(Vector3.forward, targetPosition);
            if (distanceWithTarget < MinimalForceDistance)
            {
                // Target too close.
                float force = currentAttack.Force * (MinimalForceDistance / distanceWithTarget);
                stateMachine.ForceReceiver.AddForce(-stateMachine.transform.forward * force);
            }
            //if (distanceWithTarget > currentAttack.MinimalDistance && distanceWithTarget < currentAttack.Range)
            else
            {
                // We don't apply the full force but a proportional one to the distance.
                float force = currentAttack.Force * (distanceWithTarget / currentAttack.Range) * 10;
                stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
            }
            
            alreadyAppliedForce = true;
        }

    }
}