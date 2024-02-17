using RPG.SceneManagement;
using UnityEngine;

namespace RPG.Control
{
    public class CinematicCameraController : MonoBehaviour
    {
        [SerializeField] Portal portal;
        [SerializeField] GameObject cam;

        private void Update()
        {
            EnableAnimation();
        }

        private void EnableAnimation()
        {
            if (portal.CanDoSomething)
            {
                cam.SetActive(true);
            }
        }
    }
}