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
            
            Quaternion currentRotation = stateMachine.transform.rotation;
            Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
            lookPos.y = 0f;
            stateMachine.transform.rotation = Quaternion.Slerp(currentRotation,Quaternion.LookRotation(lookPos),10f * Time.deltaTime );
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
        

        
        protected bool IsInAttackRange(int AttackId)
        {
            if (stateMachine.Player.IsDead)
            {
                return false;
            }
            // More performant to use Sqr.
            float playerDistanceSqr = (stateMachine.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
            return playerDistanceSqr <= stateMachine.Attacks[AttackId].Range * stateMachine.Attacks[AttackId].Range;
        }

        // TODO.
        protected int SelectAttack()
        {
            return -1;
        }
    }
}