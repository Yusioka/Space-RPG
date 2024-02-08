using RPG.Core;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();

            questList.CompleteObjective(quest, objective);
        }

    }
}
