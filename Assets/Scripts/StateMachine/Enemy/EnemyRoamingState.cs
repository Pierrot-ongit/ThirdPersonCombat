using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyRoamingState : EnemyBaseState
    {
        private readonly int TargetingBlendTreeHash= Animator.StringToHash("TargetingBlendTree");
        private readonly int LocomotionSpeedHash = Animator.StringToHash("Speed");
        private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
        private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
        private const float AnimatorDampTime = 0.1f;

        private float timeRoaming;
        

        private Vector3 destination;
        
        public EnemyRoamingState(EnemyStateMachine newStateMachine) : base(newStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, AnimatorDampTime);
            destination = RandomNavmeshLocation(stateMachine.EnemyData.radiusRoaming);
            stateMachine.NavMeshAgent.SetDestination(destination);
            timeRoaming = 0f;
        }

        public override void Tick(float deltaTime)
        {
            
            FacePlayer();
            MoveRoaming(deltaTime);

            if (timeRoaming > stateMachine.EnemyData.maxTimeRoaming)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            
            if (Vector3.Distance(stateMachine.transform.position, destination) < 2f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }

            UpdateAnimator(deltaTime);
            timeRoaming += deltaTime;
        }

        public override void Exit()
        {
            stateMachine.NavMeshAgent.ResetPath();
            stateMachine.NavMeshAgent.velocity = Vector3.zero;
        }

        private void MoveRoaming(float deltaTime)
        {
            if (stateMachine.NavMeshAgent.isOnNavMesh)
            {
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.EnemyData.speed, deltaTime);
            }
            stateMachine.NavMeshAgent.velocity = stateMachine.Controller.velocity;
        }
        
        public Vector3 RandomNavmeshLocation(float radius) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += stateMachine.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            // TODO Better check to avoid going to inacessible height.
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;            
            }
            return finalPosition;
        }
        
        private void UpdateAnimator(float deltaTime)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, stateMachine.Controller.velocity.x, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetingForwardHash, stateMachine.Controller.velocity.y, AnimatorDampTime, deltaTime);
        }
    }
}