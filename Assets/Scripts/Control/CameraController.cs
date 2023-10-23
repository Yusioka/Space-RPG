using UnityEngine;

namespace RPG.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform player; // ������ �� ��������� ������
        public float cameraSpeed = 2.0f; // �������� �������� ������
        public float rotationSpeed = 2.0f; // �������� �������� ������

        private Vector3 offset; // �������� ����� ������� � �������
        private bool isRotating; // ���� ��� �������� �������� ������

        void Start()
        {
            offset = transform.position - player.position;
        }

        void LateUpdate()
        {
            // ���� ������ ����� ������ ����, ������� ������ ������ ������
            if (Input.GetMouseButton(0))
            {
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.RotateAround(player.position, Vector3.up, rotationX);
                transform.RotateAround(player.position, transform.right, -rotationY);
            }

            // ���� ������ ������ ������ ����, ����� ������ ������� � ������� ������
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
