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
            questList.OnUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            if (!questList)
            {
                print("quest list is NULL");
                return;
            }

            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI uiInstance = Instantiate(questPrefab, transform);
                uiInstance.Setup(status);
            }
        }
    }
}
