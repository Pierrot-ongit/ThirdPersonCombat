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

            stateMachine.SetLastAttackTime(currentAttack, Time.time, currentAttack.IsHeavy);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if (normalizedTime >= 1f)
            {
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }
            else
            {
                if (normalizedTime >= currentAttack.ForceTime)
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
        
        private void TryApplyForce()
        {
            if (alreadyAppliedForce) return;
            
            Vector3 targetPosition = stateMachine.transform.InverseTransformPoint(stateMachine.Player.transform.position);
            // Is target in front of us ?
            float distanceWithTarget = Vector3.Dot(Vector3.forward, targetPosition);
            if (distanceWithTarget > currentAttack.MinimalDistance && distanceWithTarget < currentAttack.Range)
            {
                // We don't apply the full force but a proportional to the distance.
                float force = currentAttack.Force * (currentAttack.Range / distanceWithTarget);
                Debug.Log("Force " + force);
                stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * force);
            }
            // else if (distanceWithTarget < 0.9f)
            // {
            //     // Target too close.
            //     stateMachine.ForceReceiver.AddForce(-stateMachine.transform.forward);
            // }
            
            alreadyAppliedForce = true;
        }

    }
}