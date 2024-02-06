using UnityEngine;
using UnityEngine.Events;

namespace RPG.Quests
{
    public class QuestTrigger : MonoBehaviour
    {
        [SerializeField] UnityEvent onTrigger;

        private void OnMouseDown()
        {
            onTrigger.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                onTrigger.Invoke();
            }
        }
    }
}
