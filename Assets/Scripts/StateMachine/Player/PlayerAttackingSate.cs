using ThirdPersonCombat.Combat;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerAttackingSate : PlayerBaseState
    {
        private PlayerAttackData currentAttack;
        private bool alreadyAppliedForce;
        public PlayerAttackingSate(PlayerStateMachine newStateMachine, int AttackId) : base(newStateMachine)
        {
            currentAttack = stateMachine.Attacks[AttackId];
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransistionDuration);
            stateMachine.AttackHandler.SetCurrentAttack(currentAttack);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();
            
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

            if (normalizedTime < 1f)
            {
                if (normalizedTime >= currentAttack.ForceTime)
                {
                        TryApplyForce();
                }
                
                if (stateMachine.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else
            {
                // Go back to locomotion
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



        public override void Exit()
        {
  
        }
        
        private void TryComboAttack(float normalizedTime)
        {
            // Can we combo ?
            if (currentAttack.ComboStateIndex == -1) {return;}
            
            // Too early to chain to another combo.
            if (normalizedTime < currentAttack.ComboAttackTime) {return;}
            
            stateMachine.SwitchState(new PlayerAttackingSate(stateMachine, currentAttack.ComboStateIndex));
        }
        
        private void TryApplyForce()
        {
            if (alreadyAppliedForce) return;

            if (stateMachine.Targeter.CurrentTarget != null)
            {
                Vector3 targetPosition = stateMachine.transform.InverseTransformPoint(stateMachine.Targeter.CurrentTarget.transform.position);
                // Is target in front of us ?
                float distanceWithTarget = Vector3.Dot(Vector3.forward, targetPosition);
                float force = currentAttack.Force > 0 ? currentAttack.Force : 5;
                if (distanceWithTarget > Mathf.Max(force / 2, 3))
                {
                    stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * currentAttack.Force);
                }
                else if (distanceWithTarget < 0.9)
                {
                    // Target too close.
                    stateMachine.ForceReceiver.AddForce(-stateMachine.transform.forward * 0.2f);
                }
            }
            else
            {
                stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * currentAttack.Force);
            }
            alreadyAppliedForce = true;
        }

    }
}