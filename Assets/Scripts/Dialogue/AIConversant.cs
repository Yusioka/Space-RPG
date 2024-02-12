using RPG.Attributes;
using RPG.Control;
using RPG.Stats;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string conversantName;
        [SerializeField] Sprite conversantAvatar;

        GameObject conversant = null;

        //public bool HandleRaycast(PlayerController callingController)
        //{
        //    if (dialogue == null) return false;

        //    Health health = GetComponent<Health>();
        //    if (health && health.IsDead()) return false;

        //    if (Input.GetMouseButtonDown(0) && Vector3.Distance(transform.position, callingController.transform.position) < 3)
        //    {
        //        callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        //    }

        //    return true;
        //}

        private void OnMouseDown()
        {
            Health health = GetComponent<Health>();
            if (health &&  health.IsDead()) return;
            if (dialogue == null) return;
            GameObject player = GameObject.FindWithTag("Player");
            if (!player.GetComponent<PlayerController>().CanInteractWithComponent(gameObject)) return;
            GameObject.FindWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            conversant = gameObject;
        }

        public Sprite GetAvatar()
        {
            return conversantAvatar;
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}
