using System;
using UnityEngine;

namespace ThirdPersonCombat.Combat.Targeting
{
    public class Target : MonoBehaviour
    {
        [field:SerializeField] public GameObject LockTargetIcon { get; private set; }
        public event Action<Target> OnDestroyedEvent;

        private void OnDestroy()
        {
            UnsetLockTargetIcon();
            OnDestroyedEvent?.Invoke(this);
        }

        public void SetLockTargetIcon()
        {
            LockTargetIcon.SetActive(true);
        }
        
        public void UnsetLockTargetIcon()
        {
            LockTargetIcon.SetActive(false);
        }
        
    }
}