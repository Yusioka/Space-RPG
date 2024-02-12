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
        [SerializeField] string equipPrefabName;

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
            Transform equipObject = gameObject.Find(equipPrefabName);
            if (equipObject != null)
            {
                if (allowedEquipLocation == EquipLocation.Helmet)
                {
                    DestroyOldHelmets(gameObject);
                    equipObject.gameObject.SetActive(true);
                }

                else if (allowedEquipLocation == EquipLocation.Body)
                {
                    DestroyOldEquipment(gameObject);
                    equipObject.gameObject.SetActive(true);
                }
            }
        }

        private void DestroyOldEquipment(Transform gameObject)
        {
            foreach (Transform child in gameObject)
            {
                if (child.gameObject.tag == "Body")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        private void DestroyOldHelmets(Transform gameObject)
        {
            foreach (Transform child in gameObject)
            {
                if (child.gameObject.tag == "Helmet")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}