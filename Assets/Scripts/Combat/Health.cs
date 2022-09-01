using System;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;
        private bool isInvulnerable;
        public bool IsDead => health == 0;
        public event Action OnTakeDamage;
        public event Action OnDie;

        private void Start()
        {
            health = maxHealth;
            
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            this.isInvulnerable = isInvulnerable;
        }

        public void DealDamage(int damage)
        {
            if (health <= 0) return;

            if (isInvulnerable) return;

            health = Mathf.Max(health - damage, 0);
          //  Debug.Log(health);
            if (health <= 0)
            {
                OnDie?.Invoke();
                return;
            }

            OnTakeDamage?.Invoke();
        }
    }
}