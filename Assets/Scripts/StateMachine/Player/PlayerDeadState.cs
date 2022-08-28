using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        public PlayerDeadState(PlayerStateMachine newStateMachine) : base(newStateMachine) { }
        
        public override void Enter()
        {
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.WeaponDamage.gameObject.SetActive(false);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
        
    }
}
