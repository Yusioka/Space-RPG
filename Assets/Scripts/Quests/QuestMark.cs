using RPG.Core;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestMark : MonoBehaviour
    {
        [SerializeField] GameObject markPrefab;
        [SerializeField] bool useConditions;
        [SerializeField] Condition condition;

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
            if (!useConditions)
            {
                return false;
            }

            return condition.Check(questList.GetComponents<IPredicateEvaluator>());
        }
    }
}