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

            if (questList.HasQuest(quest) && !questList.IsObjectiveComplete(quest, objective) || string.IsNullOrEmpty(objective))
            {
                questList.CompleteObjective(quest, objective);
            }
        }

    }
}
