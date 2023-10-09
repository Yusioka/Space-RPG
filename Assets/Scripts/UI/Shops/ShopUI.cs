using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;
        [SerializeField] Transform listRoot;
        [SerializeField] RowUI rowPrefab;

        Shopper shopper = null;
        Shop currentShop = null;

        private void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
            if (shopper == null) return;
            shopper.activeShopChanged += ShopChanged;
            ShopChanged();
        }

        private void ShopChanged()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            if (currentShop == null) return;
            shopName.text = currentShop.GetShopName();

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate(rowPrefab, listRoot);
                row.Setup(item);
            }
        }

        public void CloseShop()
        {
            gameObject.SetActive(currentShop == null);
        }
    }
}
