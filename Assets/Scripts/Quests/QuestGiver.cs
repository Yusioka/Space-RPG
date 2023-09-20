using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver: MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveQuest()
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}
