using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        [SerializeField] string conversantName;

        private void OnMouseDown()
        {
            if (dialogue == null) return;

            PlayerConversant playerConversant = GameObject.FindWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.StartDialogue(this, dialogue);
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}
