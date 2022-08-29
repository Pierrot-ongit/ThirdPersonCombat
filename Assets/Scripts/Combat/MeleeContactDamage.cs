using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class MeleeContactDamage : MonoBehaviour
    {
        [SerializeField] private Collider myCollider;
        private List<Collider> alreadyCollideWith = new List<Collider>();
        private int damage;
        private float knockback;

        private void OnEnable()
        {
            alreadyCollideWith.Clear();
        }

        public void SetAttackDamage(int damage, float knockback)
        {
            this.damage = damage;
            this.knockback = knockback;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == myCollider) { return; }

            if (alreadyCollideWith.Contains(other)) return;
            alreadyCollideWith.Add(other);

            if (other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage);
            }
            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.transform.position - myCollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }
    }
}