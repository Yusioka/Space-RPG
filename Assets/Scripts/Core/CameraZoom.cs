using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] InputActionAsset inputProvider;
        [SerializeField] CinemachineFreeLook freeLookCameraToZoom;
        [SerializeField] float zoomSpeed = 1f;
        [SerializeField] float zoomAcceleration = 2.5f;
        [SerializeField] float zoomInnerRange = 5f;
        [SerializeField] float zoomOuterRange = 100f;

        float currentMiddleRigRadius = 10f;
        float newMiddleRigRadius = 10f;

        [SerializeField] float zoomYAxis = 0f;

        public float ZoomYAxis
        {
            get { return zoomYAxis; }
            set
            {
                if (zoomYAxis == value) return;
                zoomYAxis = value;
                AdjustCameraZoomIndex(ZoomYAxis);
            }
        }

        private void Awake()
        {
            inputProvider.FindActionMap("Free Look Camera Controls").FindAction("Mouse Zoom").performed += cntxt => ZoomYAxis = cntxt.ReadValue<float>();
            inputProvider.FindActionMap("Free Look Camera Controls").FindAction("Mouse Zoom").canceled += cntxt => ZoomYAxis = 0f;
        }

        //private void OnEnable()
        //{
        //    inputProvider.FindAction("Mouse Zoom").Enable();
        //}
        //private void OnDisable()
        //{
        //    inputProvider.FindAction("Mouse Zoom").Disable();
        //}

        private void LateUpdate()
        {
            UpdateZoomLevel();
        }

        private void UpdateZoomLevel()
        {
            if (currentMiddleRigRadius == newMiddleRigRadius) return;

            currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
            currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

            freeLookCameraToZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;
            freeLookCameraToZoom.m_Orbits[0].m_Height = freeLookCameraToZoom.m_Orbits[1].m_Radius;
            freeLookCameraToZoom.m_Orbits[2].m_Height = -freeLookCameraToZoom.m_Orbits[1].m_Radius;
        }

        public void AdjustCameraZoomIndex(float zoomYAxis)
        {
            if (zoomYAxis == 0) return;
            if (zoomYAxis < 0)
            {
                newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
            }
            if (zoomYAxis > 0)
            {
                newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
            }
        }
    }
}
