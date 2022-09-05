using Cinemachine;
using UnityEngine;

namespace ThirdPersonCombat.Utils
{
    public class CameraFacing : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera playerFramingCamera;

        private void Start()
        {
            playerFramingCamera = GameObject.FindGameObjectWithTag("TargetingCamera").GetComponent<CinemachineVirtualCamera>();
        }

        void LateUpdate()
        {
            transform.LookAt(2 * transform.position - playerFramingCamera.transform.position);
        }
    }
}