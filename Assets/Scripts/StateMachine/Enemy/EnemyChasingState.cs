using ThirdPersonCombat.Combat;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        private readonly int LocomotionHash= Animator.StringToHash("Locomotion");
        private readonly int LocomotionSpeedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        
        public EnemyChasingState(EnemyStateMachine newStateMachine) : base(newStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                // Transition to Idle state.
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
           
            
            if (stateMachine.nextAttack == null)
            {
                AttackData nextAttack = SelectNextAttack();
                if (nextAttack == null)
                {
                    // No attack available (cooldowns), switch to roaming.
                    stateMachine.SwitchState(new EnemyRoamingState(stateMachine));
                    return;
                }
                stateMachine.SetNextAttack(nextAttack);
            }
            
            // TODO Check for light or heavy tokens.
            if (!stateMachine.AttackTokenSubscriber.RequestToken())
            {
                stateMachine.SwitchState(new EnemyRoamingState(stateMachine));
                return;
            }
            
            if (IsInAttackRange(stateMachine.nextAttack))
            {
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine, stateMachine.nextAttack));
                return;
            }
            
            // Not yet in Attack Range.
            MoveToPlayer(deltaTime);
            FacePlayer();
            
            stateMachine.Animator.SetFloat(LocomotionSpeedHash, 1f, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.NavMeshAgent.ResetPath();
            stateMachine.NavMeshAgent.velocity = Vector3.zero;
        }

        private void MoveToPlayer(float deltaTime)
        {
            if (stateMachine.NavMeshAgent.isOnNavMesh)
            {
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.EnemyData.speed, deltaTime);
            }
            stateMachine.NavMeshAgent.velocity = stateMachine.Controller.velocity;
        }
    }
}