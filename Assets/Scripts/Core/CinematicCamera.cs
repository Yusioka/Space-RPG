using RPG.Control;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CinematicCamera : MonoBehaviour
    {
        [SerializeField] List<GameObject> cameObjectsToDisable;

        public void CameraStart()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;

            foreach (GameObject obj in cameObjectsToDisable)
            {
                obj.SetActive(false);
            }
        }
        public void CameraEnd()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;

            foreach (GameObject obj in cameObjectsToDisable)
            {
                obj.SetActive(true);
            }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
