using UnityEngine;

namespace RPG.Quests
{
    public class GuestGiver: MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveGuest()
        {
            QuestList questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}
