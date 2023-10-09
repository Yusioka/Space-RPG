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
        
        public event Action onChange;

        public IEnumerable<ShopItem> GetFilteredItems()
        { 
            foreach (StockItemConfig config in stockConfigs)
            {
                yield return new ShopItem(config.item, config.initialStock, 0, 0);
            }
        }
        public void SelectCategory(ItemCategory category) { }
        public ItemCategory GetFilter() { return ItemCategory.None; }
        public void SelectMode(bool isBuying) { }
        public bool IsBuyingMode() { return true;}
        public bool CanTransact() { return true; }
        public void ConfirmTransaction() { }
        public float TransactionTotal() { return 0; }
        public void AddToTransaction(InventoryItem item, int quantity) { }

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
