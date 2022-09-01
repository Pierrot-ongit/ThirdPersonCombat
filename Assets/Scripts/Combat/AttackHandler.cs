using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class AttackHandler : MonoBehaviour
    {
        [Tooltip("For projectile attack")]
        [field:SerializeField] public Transform ProjectileStartPosition { get; private set; }
        [SerializeField] private MeleeContactDamage[] _contactDamage;

        public AttackData CurrentAttack { get; private set; }
        public Health Target { get; private set; }

        public void SetCurrentAttack(AttackData attack)
        {
            CurrentAttack = attack;
            if (_contactDamage != null && CurrentAttack != null)
            {
                foreach (MeleeContactDamage contactDamage in _contactDamage)
                {
                    contactDamage.SetAttackDamage(CurrentAttack.Damage, CurrentAttack.Knockback);
                }
            }
        }
        
        public void SetTarget(Health target)
        {
            this.Target = target;
        }

        // Animation Event.
        public void LaunchProjectile()
        {
            if (CurrentAttack == null) { return; }
            Projectile projectileInstance = Instantiate(CurrentAttack.ProjectilePrefab, ProjectileStartPosition.position, Quaternion.identity);
            projectileInstance.SetTarget(gameObject, CurrentAttack.Damage, Target);
        }


        public void EnableMeleeContact()
        {
            if (_contactDamage != null)
            {
                foreach (MeleeContactDamage contactDamage in _contactDamage)
                {
                    contactDamage.gameObject.SetActive(true);
                }
            }
        }

        public void DisableMeleeContact()
        {
            if (_contactDamage != null)
            {
                foreach (MeleeContactDamage contactDamage in _contactDamage)
                {
                    contactDamage.gameObject.SetActive(false);
                }
            }
        }
    }
}