using UnityEngine;

namespace RPG.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Transform targetBody;
        [SerializeField] float cameraSpeed = 2.0f;
        [SerializeField] float rotationSpeed = 2.0f;

        Vector3 offset;
        GameObject player;

        [SerializeField] bool isRotating;
        [SerializeField] float minYAngle = 10.0f;
        [SerializeField] float maxYAngle = 80.0f;
        float currentX = 0.0f;
        float currentY = 0.0f;

        [SerializeField] float zoomSpeed = 2.0f;
        [SerializeField] float minZoom = 2.0f;
        [SerializeField] float maxZoom = 15.0f;

        [SerializeField] bool isMoving = false;
        float currentDistance;

        [SerializeField] MoverController controller;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Start()
        {
            if (!target) return;

            offset = transform.position - target.position;
            currentDistance = offset.magnitude;
        }

        void LateUpdate()
        {
            isMoving = target.GetComponentInParent<PlayerController>().IsMoving();

            if (!player.GetComponent<PlayerController>().IsDraggingUI)
            {
                float zoomInput = Input.GetAxis("Mouse ScrollWheel");
                currentDistance -= zoomInput * zoomSpeed;
                currentDistance = Mathf.Clamp(currentDistance, minZoom, maxZoom);
            }

            Vector3 desiredPosition = target.position - transform.forward * currentDistance;

            //
            if (isMoving && controller.IsButtonsMoving() && !Input.GetMouseButton(1))
            {
                // ��� �� ������� ����
                //desiredPosition = target.position - initialPosition * currentDistance;
                //Vector3 test = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
                //transform.position = test;

                // ���������� ����������� ���������
                desiredPosition = target.position - targetBody.forward * currentDistance + Vector3.up; 
            }
            //

            if (controller.IsButtonsMoving() && !Input.GetMouseButton(1))
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.023f);
                transform.position = smoothedPosition;
            }
            
            else
            {
                transform.position = desiredPosition;
            }


            transform.LookAt(target);

            // ���� ������ ����� ������ ����, ������� ������ ������ ������
            if (!controller.IsButtonsMoving() && Input.GetMouseButton(0))
            {
                if (player.GetComponent<PlayerController>().IsDraggingUI) return;
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

            if (controller.IsButtonsMoving())
            {
                // ���� ������ ������ ������ ����, ����� ������ ����� � ������ ������
                if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) || Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                {
                    if (player.GetComponent<PlayerController>().IsDraggingUI) return;
                    float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                    float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                    // ��������� ������ �����������, � ������� ������� ������
                    Vector3 cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f; // �������� ���������� Y, ����� ������� ������ �� �����������
                    cameraForward.Normalize(); // ����������� ������

                    if (Input.GetMouseButton(1))
                    {
                        Quaternion rotation = Quaternion.LookRotation(cameraForward);
                        targetBody.rotation = Quaternion.Slerp(targetBody.rotation, rotation, rotationSpeed * Time.deltaTime);

                        targetBody.Rotate(Vector3.up * rotationX);
                    }

                    float newRotationY = currentY - rotationY;
                    newRotationY = Mathf.Clamp(newRotationY, minYAngle, maxYAngle);
                    rotationY = currentY - newRotationY;
                    transform.RotateAround(targetBody.position, Vector3.up, rotationX);
                    transform.RotateAround(targetBody.position, transform.right, -rotationY);
                    currentY = newRotationY;
                }

                if (Input.GetMouseButton(1) && Input.GetMouseButton(0) || Input.GetMouseButton(0) && Input.GetMouseButton(1))
                {
                    if (player.GetComponent<PlayerController>().IsDraggingUI) return;
                    float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                    float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                    // ��������� ������ �����������, � ������� ������� ������
                    Vector3 cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f; // �������� ���������� Y, ����� ������� ������ �� �����������
                    cameraForward.Normalize(); // ����������� ������
                    ;
                    Quaternion rotation = Quaternion.LookRotation(cameraForward);
                    targetBody.rotation = Quaternion.Slerp(targetBody.rotation, rotation, rotationSpeed * Time.deltaTime);

                    float newRotationY = currentY - rotationY;
                    newRotationY = Mathf.Clamp(newRotationY, minYAngle, maxYAngle);
                    rotationY = currentY - newRotationY;
                    targetBody.Rotate(Vector3.up * rotationX);
                    transform.RotateAround(targetBody.position, Vector3.up, rotationX);
                    transform.RotateAround(targetBody.position, transform.right, -rotationY);
                    currentY = newRotationY;

                    float targetSpeed = targetBody.GetComponent<PlayerController>().GetSpeed();
                    targetBody.position += cameraForward * targetSpeed * Time.deltaTime; // ���������� ������ ������

                }
            }        
        }
    }
}
