using System;
using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCombat
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float drag = 0.3f;
        private Vector3 impact;
        private Vector3 dampingVelocity;
        private float verticalVelocity;

        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        private void Update()
        {
            if (verticalVelocity < 0f && controller.isGrounded)
            {
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
            if (agent != null && impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
               // agent.enabled = true;
                //agent.ResetPath();
            }
        }

        public void AddForce(Vector3 force)
        {
            impact += force;
            // Normally, only the enemies have a NavMeshAgent.
            if (agent != null)
            {
             //   agent.enabled = false;
             //agent.SetDestination(force);
            }
        }

        public void Jump(float jumpForce)
        {
            verticalVelocity += jumpForce;
        }

        public void Reset()
        {
            impact = Vector3.zero;
            verticalVelocity = 0f;
        }
    }
}