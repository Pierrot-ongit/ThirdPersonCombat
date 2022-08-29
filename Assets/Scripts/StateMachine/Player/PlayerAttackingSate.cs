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
            
            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * currentAttack.Force);
            alreadyAppliedForce = true;
        }

    }
}