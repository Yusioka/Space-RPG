using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] AudioClip audioClip = null;
        [SerializeField] KeyCode toggleKey = KeyCode.M;

        private void Start()
        {
            map.SetActive(false);
            if (secondMap != null)
            {
                secondMap.SetActive(false);
            }
            core.SetActive(true);
            miniMap.SetActive(true);
            buttons.SetActive(false);
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
            if (secondMap)
            {
                if (!map.activeSelf && !secondMap.activeSelf)
                {
                    map.SetActive(true);

                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
                }
                else if (map.activeSelf || secondMap.activeSelf)
                {
                    map.SetActive(false);
                    secondMap.SetActive(false);

                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
                }
            }

            else
            {
                map.SetActive(!map.activeSelf);
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = !map.activeSelf;
            }

            if (audioClip != null)
            {
                GetComponentInParent<AudioSource>().PlayOneShot(audioClip);
            }

            core.SetActive(!core.activeSelf);
            miniMap.SetActive(!miniMap.activeSelf);
            buttons.SetActive(!buttons.activeSelf);
        }

        public void SwitchMap()
        {
            map.SetActive(!map.activeSelf);

            if (secondMap != null)
            {
                secondMap.SetActive(!secondMap.activeSelf);
            }
        }
    }
}
