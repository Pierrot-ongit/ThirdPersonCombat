using System;
using UnityEngine;

namespace ThirdPersonCombat.Combat
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController controller;
        private Collider[] allCollider;
        private Rigidbody[] allRigidbody;
        private void Start()
        {
            allCollider = GetComponentsInChildren<Collider>(true);
            allRigidbody = GetComponentsInChildren<Rigidbody>(true);
            ToggleRagdoll(false);
        }

        public void ToggleRagdoll(bool isRagdoll)
        {
            foreach (Collider collider in allCollider)
            {
                if (collider.gameObject.CompareTag("Ragdoll"))
                {
                    collider.enabled = isRagdoll;
                }
            }
            foreach (Rigidbody rb in allRigidbody)
            {
                if (rb.gameObject.CompareTag("Ragdoll"))
                {
                    rb.isKinematic = !isRagdoll;
                    rb.useGravity = isRagdoll;
                }
            }

            controller.enabled = !isRagdoll;
            animator.enabled = !isRagdoll;
        }
    }
}