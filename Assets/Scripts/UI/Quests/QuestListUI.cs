using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI: MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        QuestList questList;

        private void Start()
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.onUpdate += Redraw;
        }

        private void Redraw()
        {
            transform.DetachChildren();
            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI uiInstance = Instantiate(questPrefab, transform);
                uiInstance.Setup(status);
            }
        }
    }
}
