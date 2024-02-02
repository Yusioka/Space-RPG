using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] GameObject map;
        [SerializeField] GameObject secondMap;
        [SerializeField] GameObject core;
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject miniMap;
        [SerializeField] KeyCode toggleKey = KeyCode.M;

        private void Start()
        {
            map.SetActive(false);
            secondMap.SetActive(false);
            core.SetActive(true);
            miniMap.SetActive(true);
            buttons.SetActive(false);
        }

        public void CameraStart()
        {
            gameObject.GetComponent<Camera>().enabled = true;
        }

        public void CameraEnd()
        {
            gameObject.GetComponent<Camera>().enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            map.SetActive(!map.activeSelf);
            core.SetActive(!core.activeSelf);
            miniMap.SetActive(!miniMap.activeSelf);
            buttons.SetActive(!buttons.activeSelf);
        }

        public void SwitchMap()
        {
            map.SetActive(!map.activeSelf);
            secondMap.SetActive(!secondMap.activeSelf);
        }
    }
}
