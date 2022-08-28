using UnityEngine;
using UnityEngine.EventSystems;

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
            else if (IsInAttackRange())
            {
                stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
                return;
            }
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
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            }
            stateMachine.NavMeshAgent.velocity = stateMachine.Controller.velocity;
        }
    }
}