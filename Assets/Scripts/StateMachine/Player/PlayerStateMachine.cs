using ThirdPersonCombat.Combat;
using ThirdPersonCombat.Combat.Targeting;
using UnityEngine;

namespace ThirdPersonCombat.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {

        [field:SerializeField] public InputReader InputReader { get; private set; }
        [field:SerializeField] public CharacterController Controller { get; private set; }
        [field:SerializeField] public Animator Animator { get; private set; }
        [field:SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field:SerializeField] public Targeter Targeter { get; private set; }
        [field:SerializeField] public AttackHandler AttackHandler { get; private set; }
        [field:SerializeField] public Health Health { get; private set; }
        [field:SerializeField] public Ragdoll Ragdoll { get; private set; }
        [field:SerializeField] public LedgeDetector LedgeDetector { get; private set; }
        [field:SerializeField] public float FreeLookMovementSpeed { get; private set; }
        [field:SerializeField] public float TargetingMovementSpeed { get; private set; }
        [field:SerializeField] public float RotationDamping { get; private set; }
        [field:SerializeField] public float DodgeLength { get; private set; }
        [field:SerializeField] public float DodgeDuration { get; private set; }
        [field:SerializeField] public float JumpForce { get; private set; }
        [field:SerializeField] public PlayerAttackData[] Attacks { get; private set; }
        [field:SerializeField] public PlayerAttackData[] HeavyAttacks { get; private set; }

        public Transform  MainCameraTransform { get; private set; }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MainCameraTransform = Camera.main.transform;
            SwitchState(new PlayerFreeLookState(this));
        }
        
        private void OnEnable()
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
            InputReader.HeavyEvent += HandleHeavyAttack;
        }
        private void OnDisable()
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
            InputReader.HeavyEvent -= HandleHeavyAttack;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new PlayerImpactState(this));
        }
        
        private void HandleDeath()
        {
            SwitchState(new PlayerDeadState(this));
        }
        
        private void HandleHeavyAttack()
        {
            if (currentState.GetType() == typeof(PlayerAttackingSate))
            {
                PlayerAttackingSate attackState = currentState as PlayerAttackingSate;
                if (attackState.CanChangeState(true))
                {
                    SwitchState(new PlayerAttackingSate(this, 0, true));
                }
            }
            SwitchState(new PlayerAttackingSate(this, 0, true));
        }
    }
}