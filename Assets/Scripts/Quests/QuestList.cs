using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        [SerializeField] AudioSource questCompletionAudio;
        public event Action OnUpdate;
        
        private readonly List<QuestStatus> statuses = new();

        private void Update()
        {
            CompleteObjectivesByPredicates();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) 
                return;

            QuestStatus newStatus = new(quest);
            statuses.Add(newStatus);

            OnUpdate?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            var status = GetQuestStatus(quest);
            
            status.CompleteObjective(objective);
            
            if (status.IsComplete())
            {
                GiveReward(quest);
                questCompletionAudio?.Play();
            }

            OnUpdate?.Invoke();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private bool IsObjectiveComplete(Quest quest, string  objective)
        {
            QuestStatus status = GetQuestStatus(quest);

            return status.IsObjectiveComplete(objective);
        }

        // находит все status квеста
        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                var success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                
                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        private void CompleteObjectivesByPredicates()
        {
            foreach (QuestStatus status in statuses)
            {            
                if (status.IsComplete()) 
                    continue;
                
                var quest = status.GetQuest();
                
                foreach (var objective in quest.GetObjectives())
                {
                    if (status.IsObjectiveComplete(objective.reference) || !objective.usesCondition) 
                        continue;
                    
                    if (objective.completionCondition.Check(GetComponents<IPredicateEvaluator>()))
                    {
                        CompleteObjective(quest, objective.reference);
                    }
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new();
            
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());          
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateList = state as List<object>;
            
            if (stateList == null) 
                return;

            statuses.Clear();
            
            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }

            OnUpdate?.Invoke();
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            // проверяет, есть ли квест, который подписан в диалоге как...
            switch (predicate)
            {
                case "HasQuest":
                    return HasQuest(Quest.GetByName(parameters[0]));
                case "CompletedQuest":
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
                case "CompletedObjective":
                    return IsObjectiveComplete(Quest.GetByName(parameters[0]), parameters[1]);
                default:
                    break;
            }
            return null;
        }
    }
}
