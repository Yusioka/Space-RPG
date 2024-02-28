using RPG.Control;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CinematicCamera : MonoBehaviour
    {
        [SerializeField] List<GameObject> camObjectsToDisable;

        public void CameraStart()
        {
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
