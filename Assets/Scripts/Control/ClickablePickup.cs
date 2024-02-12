using UnityEngine;
using RPG.Inventories;
using RPG.Core;
using RPG.Quests;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour
    {
        [SerializeField] Condition condition;

        GameObject player;
        Pickup pickup;
        QuestList playerQuestList;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();

            player = GameObject.FindGameObjectWithTag("Player");
            playerQuestList = player.GetComponent<QuestList>();
        }

        private bool CanPickup(QuestList questList)
        {
            return condition.Check(questList.GetComponents<IPredicateEvaluator>());
        }

        private void OnMouseDown()
        {
            if (!player.GetComponent<PlayerController>().CanInteractWithComponent(gameObject)) return;
            if (CanPickup(playerQuestList))
            {
                pickup.PickupItem();
            }
        }
    }
}