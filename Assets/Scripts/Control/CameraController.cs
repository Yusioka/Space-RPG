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

        Vector3 initialPosition;
        [SerializeField] MoverController controller;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Start()
        {
            if (target == null)
            {
                return;
            }
            offset = transform.position - target.position;
            currentDistance = offset.magnitude;
            initialPosition = transform.position;
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
            if (isMoving && controller.IsButtonsMoving())
            {
                // вид от первого лица
                //desiredPosition = target.position - initialPosition * currentDistance;
                //Vector3 test = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
                //transform.position = test;

                // Используем направление персонажа
                desiredPosition = target.position - targetBody.forward * currentDistance + Vector3.up; 
            }
            //

            if (controller.IsButtonsMoving())
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.023f);
                transform.position = smoothedPosition;
            }
            
            else
            {
                transform.position = desiredPosition;
            }


            transform.LookAt(target);

            // Если нажата левая кнопка мыши, вращаем камеру вокруг игрока
            if (Input.GetMouseButton(0))
            {
                if (player.GetComponent<PlayerController>().IsDraggingUI) return;
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

                currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

                // Поворачиваем камеру вокруг цели
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                float rotationDistance = Vector3.Distance(transform.position, target.position);
                Vector3 position = rotation * new Vector3(0, 0, -rotationDistance) + target.position;
                transform.rotation = rotation;
                transform.position = position;
            }

            if (controller.IsButtonsMoving())
            {
                // Если нажата правая кнопка мыши, игрок всегда смотрит в сторону камеры
                if (Input.GetMouseButton(1))
                {
                    if (player.GetComponent<PlayerController>().IsDraggingUI) return;
                    float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                    float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                    // Вычисляем вектор направления, в котором смотрит камера
                    Vector3 cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f; // Обнуляем компоненту Y, чтобы двигать только по горизонтали
                    cameraForward.Normalize(); // Нормализуем вектор
                    ;
                    Quaternion rotation = Quaternion.LookRotation(cameraForward);
                    targetBody.rotation = Quaternion.Slerp(targetBody.rotation, rotation, rotationSpeed * Time.deltaTime);

                    targetBody.Rotate(Vector3.up * rotationX);
                    transform.RotateAround(targetBody.position, Vector3.up, rotationX);
                    transform.RotateAround(targetBody.position, transform.right, -rotationY);
                }

                if (Input.GetMouseButton(1) && Input.GetMouseButton(0) || Input.GetMouseButton(0) && Input.GetMouseButton(1))
                {
                    if (player.GetComponent<PlayerController>().IsDraggingUI) return;
                    float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                    float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                    // Вычисляем вектор направления, в котором смотрит камера
                    Vector3 cameraForward = Camera.main.transform.forward;
                    cameraForward.y = 0f; // Обнуляем компоненту Y, чтобы двигать только по горизонтали
                    cameraForward.Normalize(); // Нормализуем вектор
                    ;
                    Quaternion rotation = Quaternion.LookRotation(cameraForward);
                    targetBody.rotation = Quaternion.Slerp(targetBody.rotation, rotation, rotationSpeed * Time.deltaTime);

                    targetBody.Rotate(Vector3.up * rotationX);
                    transform.RotateAround(targetBody.position, Vector3.up, rotationX);
                    transform.RotateAround(targetBody.position, transform.right, -rotationY);

                    float targetSpeed = targetBody.GetComponent<PlayerController>().GetSpeed();
                    targetBody.position += cameraForward * targetSpeed * Time.deltaTime; // Перемещаем таргет вперед
                }
            }        
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButton(1))
            {
                initialPosition = transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                transform.position = initialPosition;
            }
        }

        private void OnMouseUp()
        {
            transform.position = initialPosition;
        }
    }
}
