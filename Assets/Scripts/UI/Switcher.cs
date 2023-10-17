using UnityEngine;

namespace RPG.UI
{
    public class Switcher : MonoBehaviour
    {
        [SerializeField] GameObject entryPoint;

        private void Start()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject toDisplay)
        {
            foreach (Transform child in transform)
            {
                if (toDisplay.transform.parent != transform) return;

                child.gameObject.SetActive(child.gameObject == toDisplay);
            }
        }
    }
}
