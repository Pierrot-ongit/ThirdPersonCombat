using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int LocomotionHash= Animator.StringToHash("Locomotion");
        private readonly int LocomotionSpeedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        private float waitRemainingTime;
        
        public EnemyIdleState(EnemyStateMachine newStateMachine, bool shouldWait = false) : base(newStateMachine)
        {
            if (shouldWait)
            {
                waitRemainingTime = stateMachine.EnemyData.idleWaitCooldown;
            }
            else
            {
                waitRemainingTime = 0;
            }
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, AnimatorDampTime);
            stateMachine.AttackTokenSubscriber.ReleaseToken();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (waitRemainingTime <= 0 && IsInDetectRange())
            {
                // Transition to chasing state.
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }

            FacePlayer();
            
            stateMachine.Animator.SetFloat(LocomotionSpeedHash, 0, AnimatorDampTime, deltaTime);
            if (waitRemainingTime > 0)
            {
                waitRemainingTime = Mathf.Max(waitRemainingTime - Time.deltaTime, 0);
            }
        }

        public override void Exit()
        {

        }
    }
}