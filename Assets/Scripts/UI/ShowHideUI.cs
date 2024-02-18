using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] AudioClip audioClip = null;
        [SerializeField] bool activateOnStart = false;

        private void Start()
        {
            if (uiContainer != null)
            {
                uiContainer.SetActive(activateOnStart);
            }
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
            if (uiContainer != null)
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }

            if (audioClip != null)
            {
                GetComponentInParent<AudioSource>().PlayOneShot(audioClip);
            }
        }
    }
}
