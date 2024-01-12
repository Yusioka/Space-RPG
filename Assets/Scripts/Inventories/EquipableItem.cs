using UnityEngine;
using RPG.Core;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(fileName = "New Equipable Item", menuName = "Inventory/New Equipable Item", order = 0)]
    public class EquipableItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        [SerializeField] Condition equipCondition;

        //  [SerializeField] GameObject equipObject;
        const string equipName = "Equipment";

        // PUBLIC

        public bool CanEquip(EquipLocation equipLocation, Equipment equipment)
        {
            if (equipLocation != allowedEquipLocation) return false;

            return equipCondition.Check(equipment.GetComponents<IPredicateEvaluator>());
        }

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

        public void Equip(Transform gameObject)
        {
     //       DestroyOldEquipment(gameObject, equipName);

            string name = this.name;
            Transform equipObject = gameObject.Find(name);
            if (equipObject != null)
            {
                equipObject.gameObject.SetActive(true);
            //    equipObject.name = equipName;
            }
        }

        public void DestroyOldEquipment(Transform gameObject, string name)
        {
            Transform oldEquipment = gameObject.Find(name);
            if (oldEquipment != null)
            {
                oldEquipment.gameObject.SetActive(false);
            }
        }
    }
}