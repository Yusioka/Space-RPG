using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string conversantName;

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
            GameObject.FindWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}
