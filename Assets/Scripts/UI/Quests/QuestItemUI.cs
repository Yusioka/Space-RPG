using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            if (status == null)
                return;

            this.status = status;
            title.text = status.GetQuest().GetTitle();

            if (status.GetCompletedObjectivesCount() < status.GetQuest().GetObjectivesCount())
            {
                progress.text = status.GetCompletedObjectivesCount() + "/" + status.GetQuest().GetObjectivesCount();
            }
            else
            {
                progress.text = status.GetQuest().GetObjectivesCount() + "/" + status.GetQuest().GetObjectivesCount();
            }
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
