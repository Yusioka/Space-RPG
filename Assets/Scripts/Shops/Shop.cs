using RPG.Inventories;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] string shopName;

        [SerializeField] StockItemConfig[] stockConfigs;

        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(0, 100)]
            public float buyingDiscountPercentage;
        }

        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Shopper currentShopper = null;
        
        public event Action onChange;

        public void SetShopper(Shopper shopper)
        {
            currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        { 
            foreach (StockItemConfig config in stockConfigs)
            {
                float price = config.item.GetPrice() * (1 - config.buyingDiscountPercentage / 100);
                int quantityInTransaction = 0;
                transaction.TryGetValue(config.item, out quantityInTransaction);
                yield return new ShopItem(config.item, config.initialStock, price, quantityInTransaction);
            }
        }
        public void SelectCategory(ItemCategory category) { }
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) { }
        public bool IsBuyingMode() { return true;}
        public bool CanTransact() { return true; }
        public void ConfirmTransaction()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return;

            var transactionSnapshot = new Dictionary<InventoryItem, int>(transaction);
            foreach (InventoryItem item in transactionSnapshot.Keys)
            {
                int quantity = transactionSnapshot[item];
                for (int i = 0; i < quantity; i++)
                {
                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        AddToTransaction(item, -1);
                    }
                }
            }
        }
        public float TransactionTotal() { return 0; }
        public void AddToTransaction(InventoryItem item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }
            transaction[item] += quantity;

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if (onChange != null)
            {
                onChange();
            }
        }

        public string GetShopName()
        { 
            return shopName;
        }

        private void OnMouseDown()
        {
            GameObject.FindWithTag("Player").GetComponent<Shopper>().SetActiveShop(this);
        }
    }
}
