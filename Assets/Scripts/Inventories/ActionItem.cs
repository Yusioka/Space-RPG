using UnityEngine;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory item that can be placed in the action bar and "Used".
    /// </summary>
    /// <remarks>
    /// This class should be used as a base. Subclasses must implement the `Use`
    /// method.
    /// </remarks>
    [CreateAssetMenu(fileName = "New Action Item", menuName = "RPG Project/Inventory/New Action Item", order = 0)]
    public class ActionItem : InventoryItem
    {
        // CONFIG DATA
        [Tooltip("Does an instance of this item get consumed every time it's used.")]
        [SerializeField] bool consumable = false;

        // PUBLIC

        /// <summary>
        /// Trigger the use of this item. Override to provide functionality.
        /// </summary>
        /// <param name="user">The character that is using this action.</param>
        public virtual bool Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
            return isConsumable();
        }

        public bool isConsumable()
        {
            return consumable;
        }
    }
}