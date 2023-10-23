using UnityEngine;

namespace RPG.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform player; // Ссылка на трансформ игрока
        public float cameraSpeed = 2.0f; // Скорость движения камеры
        public float rotationSpeed = 2.0f; // Скорость вращения камеры

        private Vector3 offset; // Смещение между камерой и игроком
        private bool isRotating; // Флаг для проверки вращения камеры

        void Start()
        {
            offset = transform.position - player.position;
        }

        void LateUpdate()
        {
            // Если нажата левая кнопка мыши, вращаем камеру вокруг игрока
            if (Input.GetMouseButton(0))
            {
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.RotateAround(player.position, Vector3.up, rotationX);
                transform.RotateAround(player.position, transform.right, -rotationY);
            }

            // Если нажата правая кнопка мыши, игрок всегда смотрит в сторону камеры
            if (Input.GetMouseButton(1))
            {
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;
                player.Rotate(Vector3.up * rotationX);

                transform.RotateAround(player.position, Vector3.up, rotationX);
                transform.RotateAround(player.position, transform.right, -rotationY);
            }
        }
    }
}
