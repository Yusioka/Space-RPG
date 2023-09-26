using UnityEngine;
using RPG.Inventories;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        private void OnMouseDown()
        {
            pickup.PickupItem();
        }

        //public bool HandleRaycast(PlayerController callingController)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        pickup.PickupItem();
        //    }
        //    return true;
        //}
    }
}