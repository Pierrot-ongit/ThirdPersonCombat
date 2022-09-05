using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ThirdPersonCombat.Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
        private Camera mainCamera;
        private List<Target> targets = new List<Target>();
        public Target CurrentTarget { get; private set; }

        private void Start()
        {
            mainCamera = Camera.main;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Target>(out Target target))
            {
                targets.Add(target);
                target.OnDestroyedEvent += RemoveTarget;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Target>(out Target target))
            {
                RemoveTarget(target);
            }
        }

        public bool SelectTarget()
        {
            if (targets.Count == 0) { return false;}

            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;
            
            foreach (Target target in targets)
            {
               Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
               if (!target.GetComponentInChildren<Renderer>().isVisible)
               {
                   continue;
               }

               // Distance between the target position on screen and the middle of the screen.
               Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
               if (toCenter.sqrMagnitude < closestTargetDistance)
               {
                   closestTarget = target;
                   closestTargetDistance = toCenter.sqrMagnitude;
               }
            }

            if (closestTarget == null) {return false;}
            
            CurrentTarget = closestTarget;
            cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
            CurrentTarget.SetLockTargetIcon();
            return true;
        }

        public void CancelTargeting()
        {
            if (CurrentTarget == null) {return;}
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget.UnsetLockTargetIcon();
            CurrentTarget = null;
        }

        public void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                cinemachineTargetGroup.RemoveMember(target.transform);
                CurrentTarget.UnsetLockTargetIcon();
                CurrentTarget = null;
            }

            target.OnDestroyedEvent -= RemoveTarget;
            targets.Remove(target);
        }
    }
}