using RPG.Inventories;
using System;
using UnityEngine;

namespace RPG.Shops
{
    public class ShopItem : MonoBehaviour
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            this.price = price;
            this.quantityInTransaction = quantityInTransaction;
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }
        public string GetName()
        {
            return item.GetDisplayName();
        }
        internal int GetAvailability()
        {
            return availability;
        }
        internal float GetPrice()
        {
            return item.GetPrice();
        }
        internal int GetQuantity()
        {
            return quantityInTransaction;
        }
    }
}
