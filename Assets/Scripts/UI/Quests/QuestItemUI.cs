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
            this.status = status;
            title.text = status.GetQuest().GetTitle();

            if (status.GetCompleteObjectivesCount() < status.GetQuest().GetObjectiveCount())
            {
                progress.text = status.GetCompleteObjectivesCount() + "/" + status.GetQuest().GetObjectiveCount();
            }
            else
            {
                progress.text = status.GetQuest().GetObjectiveCount() + "/" + status.GetQuest().GetObjectiveCount();
            }
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
