using Cinemachine;
using UnityEngine;

namespace ThirdPersonCombat.Utils
{
    public class RotateOverTime : MonoBehaviour
    {
        [SerializeField] float RotationSpeed;

        void LateUpdate()
        {
            transform.Rotate( Vector3.forward * (RotationSpeed * Time.deltaTime));
        }
    }
}