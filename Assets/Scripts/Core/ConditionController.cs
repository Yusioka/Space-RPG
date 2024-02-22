using RPG.Quests;
using UnityEngine;

namespace RPG.Core
{
    public class ConditionController : MonoBehaviour
    {
        [SerializeField] BoxCollider objectToControl;
        [SerializeField] Condition condition;

        QuestList playerQuestList;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerQuestList = player.GetComponent<QuestList>();
        }

        private bool CanEnableObject(QuestList questList)
        {
            return condition.Check(questList.GetComponents<IPredicateEvaluator>());
        }

        private void Update()
        {
            if (playerQuestList == null)
            {
                Destroy(gameObject);
            }

            EnableObject(objectToControl);
        }

        private void EnableObject(BoxCollider collider)
        {
            if (CanEnableObject(playerQuestList))
            {
                collider.enabled = true;
            }
            else
            {
                collider.enabled = false;
            }
        }
    }
}
