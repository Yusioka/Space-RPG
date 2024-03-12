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

            progress.text = status.GetCompletedObjectivesCount() + "/" + status.GetQuest().GetObjectivesCount();
            //if (status.GetCompleteObjectivesCount() < status.GetQuest().GetObjectiveCount())
            //{
            //    progress.text = status.GetCompleteObjectivesCount() + "/" + status.GetQuest().GetObjectiveCount();
            //}
            //else
            //{
            //    progress.text = status.GetQuest().GetObjectiveCount() + "/" + status.GetQuest().GetObjectiveCount();
            //}
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
