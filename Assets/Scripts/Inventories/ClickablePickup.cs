using RPG.Combat;
using System.Collections;
using UnityEngine;

namespace RPG.Inventories
{
    public class ClickablePickup : MonoBehaviour
    {
        [SerializeField] InventoryItem item = null;
        [SerializeField] float respawnTime = 3f;

        Inventory inventory;

        private void Awake()
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                inventory.AddItemToSlot(0, item, 1);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }


        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
