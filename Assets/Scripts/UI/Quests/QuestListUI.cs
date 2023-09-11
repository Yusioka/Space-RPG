using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI: MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;

        private void Start()
        {
            transform.DetachChildren();
            foreach (Quest quest in tempQuests)
            {
                QuestItemUI uiInstance = Instantiate(questPrefab, transform);
                uiInstance.Setup(quest);
            }
        }
    }
}
