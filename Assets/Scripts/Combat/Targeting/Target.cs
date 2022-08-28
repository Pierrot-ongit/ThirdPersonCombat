using System;
using UnityEngine;

namespace ThirdPersonCombat.Combat.Targeting
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnDestroyedEvent;

        private void OnDestroy()
        {
            OnDestroyedEvent?.Invoke(this);
        }
    }
}