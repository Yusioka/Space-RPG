using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestMark : MonoBehaviour
    {
        [SerializeField] GameObject markPrefab;
        [SerializeField] bool useConditions;
        [SerializeField] List<Condition> conditions;

        QuestList questList;
        Transform mainCameraTransform;

        private void Awake()
        {
            mainCameraTransform = Camera.main.transform;

            var player = GameObject.FindGameObjectWithTag("Player");
            questList = player.GetComponent<QuestList>();
        }

        private void Update()
        {
            markPrefab.transform.LookAt(mainCameraTransform);
            markPrefab.SetActive(CanActivateMark(questList));
        }

        private bool CanActivateMark(QuestList questList)
        {
            bool result = false;

            if (!useConditions)
            {
                return false;
            }

            foreach (Condition condition in conditions)
            {
                result = condition.Check(questList.GetComponents<IPredicateEvaluator>());

                if (result) return true;
            }

            return result;
        }
    }
}