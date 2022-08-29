using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(EnemyStateMachine newStateMachine) : base(newStateMachine) { }

        public override void Enter()
        {
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.AttackHandler.DisableMeleeContact();
            GameObject.Destroy(stateMachine.Target);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
        
    }
}
