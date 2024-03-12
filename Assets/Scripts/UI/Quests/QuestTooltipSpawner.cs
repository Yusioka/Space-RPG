using RPG.Core.UI.Tooltips;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        int objectivesCounter;

        public override bool CanCreateTooltip()
        {
            objectivesCounter = 0;
            QuestStatus status = GetComponent<QuestItemUI>().GetQuestStatus();
            Quest quest = status.GetQuest();

            foreach (var objective in quest.GetObjectives())
            {
                if (string.IsNullOrEmpty(objective.reference))
                {
                    objectivesCounter++;
                }
            }

            if (objectivesCounter == quest.GetObjectivesCount())
            {
                return false;
            }
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = GetComponent<QuestItemUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(status);
        }
    }
}
