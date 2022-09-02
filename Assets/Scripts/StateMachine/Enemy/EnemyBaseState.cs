using ThirdPersonCombat.Combat;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        public EnemyBaseState(EnemyStateMachine newStateMachine)
        {
            stateMachine = newStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        
        protected void FacePlayer()
        {
            if (stateMachine.Player == null) return;
            FaceDestination(stateMachine.Player.transform.position);
        }
        
        protected void FaceDestination(Vector3 destination)
        {
            Quaternion currentRotation = stateMachine.transform.rotation;
            Vector3 lookPos = destination - stateMachine.transform.position;
            lookPos.y = 0f;
            //stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
            stateMachine.transform.rotation = Quaternion.Slerp(currentRotation,Quaternion.LookRotation(lookPos),100f * Time.deltaTime );
        }
        
        
        protected bool IsInDetectRange()
        {
            if (stateMachine.Player.IsDead)
            {
                return false;
            }
            // More performant to use Sqr.
            float playerDistanceSqr = (stateMachine.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.EnemyData.detectRange * stateMachine.EnemyData.detectRange;
        }
        
        protected bool IsInChaseRange()
        {
            if (stateMachine.Player.IsDead)
            {
                return false;
            }
            // More performant to use Sqr.
            float playerDistanceSqr = (stateMachine.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.EnemyData.chasingRange * stateMachine.EnemyData.chasingRange;
        }
        

        
        protected bool IsInAttackRange(AttackData attack)
        {
            if (stateMachine.Player.IsDead)
            {
                return false;
            }
            // More performant to use Sqr.
            float playerDistanceSqr = (stateMachine.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
            return playerDistanceSqr <= attack.Range * attack.Range
                   && playerDistanceSqr >= attack.MinimalDistance * attack.MinimalDistance;
        }


        protected AttackData SelectNextAttack()
        {
            foreach (AttackData attackData in stateMachine.HeavyAttacks)
            {
                if (Time.time - stateMachine.HeavyCooldowns[attackData] > attackData.Cooldown)
                {
                    if (IsInAttackRange(attackData))
                    {
                        return attackData;
                    }
                }
            }
            
            
            foreach (AttackData attackData in stateMachine.Attacks)
            {
                if (Time.time - stateMachine.Cooldowns[attackData] > attackData.Cooldown)
                {
                    if (IsInAttackRange(attackData))
                    {
                        return attackData;
                    }
                }
            }

            return null;
        }
    }
}