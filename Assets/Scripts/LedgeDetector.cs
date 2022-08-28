using System;
using UnityEngine;

namespace ThirdPersonCombat
{
    public class LedgeDetector : MonoBehaviour
    {
        public event Action<Vector3, Vector3> OnLedgeDetect; 
        
        // Layers params make it LedgeDetector only collide with Ledges.
        private void OnTriggerEnter(Collider other)
        {
            OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
        }
    }
}