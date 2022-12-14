using System;
using System.Collections.Generic;
using ThirdPersonCombat.Combat;
using ThirdPersonCombat.Combat.Enemy;
using ThirdPersonCombat.Combat.Targeting;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat.StateMachine.Enemy
{
    public class EnemyStateMachine : StateMachine
    {
        public bool DrawGizmos;
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public SFXManager SFXManager { get; private set; }
        [field:SerializeField] public CharacterController Controller { get; private set; }
        [field:SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field:SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
        [field:SerializeField] public AttackHandler AttackHandler { get; private set; }
        [field:SerializeField] public AttackTokenSubscriber AttackTokenSubscriber { get; private set; }
        [field:SerializeField] public Health Health { get; private set; }
        [field:SerializeField] public Target Target { get; private set; }
        [field:SerializeField] public Ragdoll Ragdoll { get; private set; }
        [field:SerializeField] public EnemyData EnemyData { get; private set; }
        [field:SerializeField] public AttackData[] Attacks { get; private set; }
        [field:SerializeField] public AttackData[] HeavyAttacks { get; private set; }

        public Dictionary<AttackData, float> Cooldowns { get; private set; }
        public Dictionary<AttackData, float> HeavyCooldowns { get; private set; }
        public AttackData nextAttack { get; private set; }

        public Health Player { get; private set; }
        private void Start()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Health>();
            
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;

            Cooldowns = new Dictionary<AttackData, float>();
            foreach (AttackData attack in Attacks)
            {
                Cooldowns[attack] = 0;
            }
            HeavyCooldowns = new Dictionary<AttackData, float>();
            foreach (AttackData attack in HeavyAttacks)
            {
                HeavyCooldowns[attack] = 0;
            }

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

        public void SetLastAttackTime(AttackData attack, float time, bool heavy = false)
        {
            if (heavy)
            {
                HeavyCooldowns[attack] = time;
            }
            else
            {
                Cooldowns[attack] = time;
            }
        }

        public void SetNextAttack(AttackData attackData)
        {
            nextAttack = attackData;
        }

        private void OnDrawGizmosSelected()
        {
            if (!DrawGizmos) return;
            //Gizmos.color = Color.red;
          //  Gizmos.DrawWireSphere(transform.position, EnemyData.detectRange);
        }
        
        private void OnDrawGizmos()
        {
            if (!DrawGizmos) return;
            if (currentState == null) return;
            System.Type stateType = currentState.GetType();
            if (stateType == typeof(EnemyAttackingState))
            {
                Gizmos.color = Color.red;
                EnemyAttackingState AttackState = currentState as EnemyAttackingState;
             //   Gizmos.DrawWireSphere(transform.position, AttackState.GetAttackRange());
            }
            else if (stateType == typeof(EnemyChasingState))
            {
                Gizmos.color = Color.yellow;
            }
            else if (stateType == typeof(EnemyRoamingState))
            {
                Gizmos.color = Color.blue;
            }
            else if (stateType == typeof(EnemyIdleState))
            {
                Gizmos.color = Color.green;
            }
            else if (stateType == typeof(EnemyImpactState))
            {
                Gizmos.color = Color.grey;
            }

            Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.2f);
        }
    }
}