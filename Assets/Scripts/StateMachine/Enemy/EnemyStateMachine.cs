using System;
using ThirdPersonCombat.Combat;
using ThirdPersonCombat.Combat.Enemy;
using ThirdPersonCombat.Combat.Targeting;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public CharacterController Controller { get; private set; }
        [field:SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field:SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field:SerializeField] public AttackHandler AttackHandler { get; private set; }
        [field:SerializeField] public Health Health { get; private set; }
        [field:SerializeField] public Target Target { get; private set; }
        [field:SerializeField] public Ragdoll Ragdoll { get; private set; }
        [field:SerializeField] public EnemyData EnemyData { get; private set; }
        [field:SerializeField] public AttackData[] Attacks { get; private set; }

        // TODO A remplacer par Dictionary, for every Attacks.
        public float PreviousAttackTime { get; private set; } = Mathf.NegativeInfinity;

        public Health Player { get; private set; }
        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;
            
            SwitchState(new EnemyIdleState(this));
        }

        private void OnEnable()
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void OnDisable()
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new EnemyImpactState(this));
        }
        
        private void HandleDeath()
        {
            SwitchState(new EnemyDeadState(this));
        }

        public void SetLastAttackTime(float time)
        {
            PreviousAttackTime = time;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyData.detectRange);
        }
    }
}