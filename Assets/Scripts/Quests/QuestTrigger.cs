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
    }
}
