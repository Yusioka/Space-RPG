using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completeObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completeObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completeObjectives = state.completeObjectives;
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completeObjectives.Add(objective);
            }
        }
        public bool IsComplete()
        {
            foreach (var objective in quest.GetObjectives())
            {
                if (!completeObjectives.Contains(objective.reference))
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
        public int GetCompleteObjectivesCount()
        {
            return completeObjectives.Count;
        }
        public bool IsObjectiveComplete(string objective)
        {
            // если completeObjectives плеера == обджективам скриптабл обджекта quest
            return completeObjectives.Contains(objective);
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completeObjectives = completeObjectives;
            return state;
        }
    }
}
