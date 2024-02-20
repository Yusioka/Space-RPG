using UnityEngine;

namespace RPG.Core
{
    public class CinematicCamera : MonoBehaviour
    {
        public void CameraStart() { }
        public void CameraEnd()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
