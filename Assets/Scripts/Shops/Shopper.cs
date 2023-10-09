using System;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        Shop activeShop = null;

        public event Action activeShopChanged;

        public void SetActiveShop(Shop shop)
        {
            activeShop = shop;
            if (activeShopChanged != null)
            {
                activeShopChanged();
            }
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}
