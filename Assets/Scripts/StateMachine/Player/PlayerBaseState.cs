using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public  abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        public PlayerBaseState(PlayerStateMachine newStateMachine)
        {
            stateMachine = newStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            if (stateMachine.Controller.enabled)
            {
                stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
            }
        }
        
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        
        protected void FaceTarget()
        {
            if (stateMachine.Targeter.CurrentTarget == null) {return;}
            Quaternion currentRotation = stateMachine.transform.rotation;

            Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
            lookPos.y = 0f;
            stateMachine.transform.rotation = Quaternion.Slerp(currentRotation,Quaternion.LookRotation(lookPos),10f * Time.deltaTime );
        }

        protected void ReturnToLocomotion()
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        
    }
}