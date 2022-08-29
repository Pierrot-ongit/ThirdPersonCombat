using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class AttackHandler : MonoBehaviour
    {
        [Tooltip("For projectile attack")]
        [field:SerializeField] public Transform StartPosition { get; private set; }
        [SerializeField] private MeleeContactDamage _contactDamage;

        public AttackData CurrentAttack { get; private set; }
        public Health Target { get; private set; }

        public void SetCurrentAttack(AttackData attack)
        {
            CurrentAttack = attack;
            if (CurrentAttack != null)
            {
                _contactDamage.SetAttackDamage(CurrentAttack.Damage, CurrentAttack.Knockback);
            }
        }
        
        public void SetTarget(Health target)
        {
            this.Target = target;
        }

        public void LaunchProjectile()
        {
            if (CurrentAttack == null) { return; }
            Projectile projectileInstance = Instantiate(CurrentAttack.ProjectilePrefab, StartPosition.position, Quaternion.identity);
            projectileInstance.SetTarget(gameObject, CurrentAttack.Damage, Target);
        }


        public void EnableMeleeContact()
        {
            _contactDamage.gameObject.SetActive(true);
        }

        public void DisableMeleeContact()
        {
            _contactDamage.gameObject.SetActive(false);
        }
    }
}