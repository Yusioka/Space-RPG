using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completedObjectives = state.completedObjectives;
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                foreach (var obj in completedObjectives)
                {
                    if (obj == objective) return;
                }

                completedObjectives.Add(objective);
            }
        }
        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if (!completedObjectives.Contains(objective.reference))
                {
                    return false;
                }
            }
            return true;
        }

        public Quest GetQuest()
        {
            return quest;
        }
        public int GetCompletedObjectivesCount()
        {
            return completedObjectives.Count;
        }
        public bool IsObjectiveComplete(string objective)
        {
            // если completedObjectives плеера == обджективам скриптабл обджекта quest
            return completedObjectives.Contains(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }
    }
}
