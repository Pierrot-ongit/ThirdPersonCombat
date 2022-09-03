using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonCombat.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 5f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent ontHit;

        int damage = 0;
        GameObject instigator = null;
        Vector3 targetPoint;
        private Health target;


        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, int damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;
            this.targetPoint = targetPoint;

            Destroy(gameObject, maxLifeTime);
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }
            CharacterController targetController = target.GetComponent<CharacterController>();
            if (targetController != null)
            {
                return target.transform.position + targetController.center;
            }
            
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            // We collided with ourselves (player).
            if (other.gameObject == instigator) return;
            
            Health health = other.GetComponent<Health>();
            // If we have a "living" target, we ignore this collision.
            if (target != null && health != target) return;

            // We collided with something that can't be damaged (not living).
            if (health == null || health.IsDead) return;
            
            speed = 0;
            health.DealDamage(this.damage);
            ontHit.Invoke();
            transform.parent = other.transform; // We stuck the projectile to the target.
            
            if (hitEffect != null)
            {
                GameObject hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
            }
            
            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
        
    }
}

