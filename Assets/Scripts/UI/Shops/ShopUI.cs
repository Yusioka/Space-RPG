using RPG.Shops;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        Shopper shopper = null;
        Shop currentShop = null;

        private void Start()
        {
            gameObject.SetActive(false);
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null )
            {
                return;
            }
            else
            {
                shopper.activeShopChanged += ShopChanged;
            }
        }

        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
        }
    }
}
