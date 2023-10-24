using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target; // ������ �� ��������� ������
        [SerializeField] Transform targetBody;
        [SerializeField] float cameraSpeed = 2.0f; // �������� �������� ������
        [SerializeField] float rotationSpeed = 2.0f; // �������� �������� ������

        private Vector3 offset; // �������� ����� ������� � �������

        [SerializeField] bool isRotating; // ���� ��� �������� �������� ������
        [SerializeField] float minYAngle = 10.0f; // ����������� ���� ������� �� ���������
        [SerializeField] float maxYAngle = 80.0f; // ������������ ���� ������� �� ���������
        private float currentX = 0.0f;
        private float currentY = 0.0f;

        [SerializeField] float zoomSpeed = 2.0f; // �������� ��������� �����������/���������
        [SerializeField] float minZoom = 2.0f; // ����������� ���������� �����������
        [SerializeField] float maxZoom = 15.0f; // ������������ ���������� ���������

        [SerializeField] bool isMoving = false; // ���� ��� �������� �������� ���������
        private float currentDistance;


        void Start()
        {
            if (target == null || target.tag != "Player")
            {
                return; // ���� ���� �����������, ������ �� ������
            }
            offset = transform.position - target.position;
            currentDistance = offset.magnitude;
        }

        void LateUpdate()
        {
            //
            // ��������� �������� ���������
            if (target.GetComponentInParent<PlayerController>().IsMoving())
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            //

            // �������� �������� �������� �������� ����
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");
            currentDistance -= zoomInput * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minZoom, maxZoom);

            Vector3 desiredPosition = target.position - transform.forward * currentDistance;

            //
            if (isMoving)
            {
               ///
            }
            //

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
            transform.position = smoothedPosition;


            // ����������� ������ ������ ������� �� ����
            transform.LookAt(target);

            // ���� ������ ����� ������ ����, ������� ������ ������ ������
            if (Input.GetMouseButton(0))
            {
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

                currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

                // ������������ ������ ������ ����
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                float rotationDistance = Vector3.Distance(transform.position, target.position);
                Vector3 position = rotation * new Vector3(0, 0, -rotationDistance) + target.position;
                transform.rotation = rotation;
                transform.position = position;
            }

            // ���� ������ ������ ������ ����, ����� ������ ������� � ������� ������
            if (Input.GetMouseButton(1))
            {
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;
                targetBody.Rotate(Vector3.up * rotationX);

                transform.RotateAround(targetBody.position, Vector3.up, rotationX);
                transform.RotateAround(targetBody.position, transform.right, -rotationY);
            }
        }
    }
}
