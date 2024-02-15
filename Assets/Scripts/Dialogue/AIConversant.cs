using RPG.Attributes;
using RPG.Control;
using RPG.Stats;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string conversantName;
        [SerializeField] Sprite conversantAvatar;

        GameObject conversant = null;
        GameObject player;

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
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            //if (Vector3.Distance(this.gameObject.transform.position, GameObject.FindWithTag("Player").transform.position)>5)
            //{
            //    GameObject.FindWithTag("Player").GetComponent<PlayerConversant>().Quit();
            //}
        }

        private void OnMouseDown()
        {
            if (this.enabled == false) return;
            Health health = GetComponent<Health>();
            if (health &&  health.IsDead()) return;
            if (dialogue == null) return;
            if (!player.GetComponent<PlayerController>().CanInteractWithComponent(gameObject)) return;
            GameObject.FindWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
         //   conversant = gameObject;
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
