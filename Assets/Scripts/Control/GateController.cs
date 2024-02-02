using RPG.Core;
using RPG.Dialogue;
using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] Condition condition;

        //public bool CanOpen(EquipLocation equipLocation, Equipment equipment)
        //{
        //    if (equipLocation != allowedEquipLocation) return false;

        //    return condition.Check(equipment.GetComponents<IPredicateEvaluator>());
        //}


        //public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        //{
        //    return condition.Check(evaluators);
        //}

        //public IEnumerable<DialogueNode> GetChoices()
        //{
        //    foreach (DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
        //    {
        //        if (node.CheckCondition(GetEvaluators()))
        //        {
        //            // yield - как break, только с возвращаемым значением
        //            yield return node;
        //        }
        //    }
        //}

        //public int MaxAcceptable(InventoryItem item)
        //{
        //    EquipableItem equipableItem = item as EquipableItem;
        //    if (equipableItem == null) return 0;
        //    if (!equipableItem.CanEquip(equipLocation, playerEquipment)) return 0;
        //    if (GetItem() != null) return 0;

        //    return 1;
        //}
    }
}
