using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
    public class CameraController : MonoBehaviour
    {
        CinemachineVirtualCamera virtualCamera;
        CinemachineComponentBase componentBase;

        [SerializeField] Transform target;
        [SerializeField] float sensitivity = 3;
        float horizontal = 0;
        float vertical = -33;

        float cameraDistance;

        private void Start()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            RotateCamera();
        //    ZoomCamera();
        }

        //private void ZoomCamera()
        //{
        //    if (componentBase == null)
        //    {
        //        componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        //    }
        //    if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //    {
        //        cameraDistance = Input.GetAxis("Mouse ScrollWheel") * sensitivity * 5;
        //        if (componentBase is CinemachineFramingTransposer)
        //        {
        //            if ((componentBase as CinemachineFramingTransposer).m_CameraDistance <= 4 && cameraDistance > 0) return;
        //            else if ((componentBase as CinemachineFramingTransposer).m_CameraDistance >= 20 && cameraDistance < 0) return;
        //            (componentBase as CinemachineFramingTransposer).m_CameraDistance -= cameraDistance;
        //        }
        //    }
        //}

        private void RotateCamera()
        {
            if (Input.GetMouseButton(1))
            {
                horizontal = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
                vertical += Input.GetAxis("Mouse Y") * sensitivity;
                vertical = Mathf.Clamp(vertical, -80, 0);
                transform.localEulerAngles = new Vector3(-vertical, horizontal, 0);
                transform.position = transform.localRotation * Vector3.zero + target.position;
            }
        }
    }
}