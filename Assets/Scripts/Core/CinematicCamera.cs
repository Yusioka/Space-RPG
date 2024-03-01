using RPG.Control;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class CinematicCamera : MonoBehaviour
    {
        [SerializeField] List<GameObject> camObjectsToDisable;

        public UnityEvent enableCamera;
        public bool wasEnabled { get; set; }

        private void Update()
        {
            if (!wasEnabled)
            {
                enableCamera.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CameraEnd();
            }
        }

        public void CameraStart()
        {
            wasEnabled = true;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
            Cursor.visible = false;

            foreach (GameObject obj in camObjectsToDisable)
            {
                obj.SetActive(false);
            }
        }
        public void CameraEnd()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
            Cursor.visible = true;

            foreach (GameObject obj in camObjectsToDisable)
            {
                obj.SetActive(true);
            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
