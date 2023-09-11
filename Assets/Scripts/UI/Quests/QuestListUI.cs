using UnityEngine;

namespace RPG.Quests
{
    public class QuestListUI: MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;

        private void Start()
        {
            foreach (Quest quest in tempQuests)
            {
                QuestItemUI uiInstance = Instantiate(questPrefab, transform);
                uiInstance.Setup(quest);
            }
        }
    }
}
