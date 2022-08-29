using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly int LocomotionHash= Animator.StringToHash("Locomotion");
        private readonly int LocomotionSpeedHash = Animator.StringToHash("Speed");
        private const float AnimatorDampTime = 0.1f;
        
        public EnemyIdleState(EnemyStateMachine newStateMachine) : base(newStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, AnimatorDampTime);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (IsInDetectRange())
            {
                // Transition to chasing state.
                stateMachine.SwitchState(new EnemyChasingState(stateMachine));
                return;
            }

            FacePlayer();
            
            stateMachine.Animator.SetFloat(LocomotionSpeedHash, 0, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {

        }
    }
}