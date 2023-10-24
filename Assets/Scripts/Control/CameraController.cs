using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform target; // Ссылка на трансформ игрока
        [SerializeField] Transform targetBody;
        [SerializeField] float cameraSpeed = 2.0f; // Скорость движения камеры
        [SerializeField] float rotationSpeed = 2.0f; // Скорость вращения камеры

        private Vector3 offset; // Смещение между камерой и игроком

        [SerializeField] bool isRotating; // Флаг для проверки вращения камеры
        [SerializeField] float minYAngle = 10.0f; // Минимальный угол наклона по вертикали
        [SerializeField] float maxYAngle = 80.0f; // Максимальный угол наклона по вертикали
        private float currentX = 0.0f;
        private float currentY = 0.0f;

        [SerializeField] float zoomSpeed = 2.0f; // Скорость изменения приближения/отдаления
        [SerializeField] float minZoom = 2.0f; // Минимальное расстояние приближения
        [SerializeField] float maxZoom = 15.0f; // Максимальное расстояние отдаления

        [SerializeField] bool isMoving = false; // Флаг для проверки движения персонажа
        private float currentDistance;


        void Start()
        {
            if (target == null || target.tag != "Player")
            {
                return; // Если цель отсутствует, ничего не делаем
            }
            offset = transform.position - target.position;
            currentDistance = offset.magnitude;
        }

        void LateUpdate()
        {
            //
            // Проверяем движение персонажа
            if (target.GetComponentInParent<PlayerController>().IsMoving())
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            //

            // Получаем величину вращения колесика мыши
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


            // Направление камеры всегда смотрит на цель
            transform.LookAt(target);

            // Если нажата левая кнопка мыши, вращаем камеру вокруг игрока
            if (Input.GetMouseButton(0))
            {
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

            // Если нажата правая кнопка мыши, игрок всегда смотрит в сторону камеры
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
