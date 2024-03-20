using RPG.Quests;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class ConditionController : MonoBehaviour
    {
        [SerializeField] BoxCollider objectToControl;
        [SerializeField] GameObject gameObjectToControl;
        [SerializeField] Condition condition;

        QuestList playerQuestList;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (!player) return;
            playerQuestList = player.GetComponent<QuestList>();
        }

        private bool CanEnableObject(QuestList questList)
        {
            return condition.Check(questList.GetComponents<IPredicateEvaluator>());
        }

        private void Update()
        {
            if (objectToControl)
            {
                EnableCollider(objectToControl);
            }

            if (!gameObjectToControl) return;
            StartCoroutine(EnableGameObject(gameObjectToControl));
        }

        private void EnableCollider(BoxCollider collider)
        {
            if (!playerQuestList) return;

            if (CanEnableObject(playerQuestList))
            {
                collider.enabled = true;
            }
            else
            {
                collider.enabled = false;
            }
        }

        private IEnumerator EnableGameObject(GameObject gameObjectToControl)
        {
            yield return new WaitForSeconds(5);
            if (!playerQuestList) yield return null;

            if (CanEnableObject(playerQuestList))
            {
                gameObjectToControl.active = true;
            }
            else
            {
                gameObjectToControl.active = false;
            }
        }
    }
}
