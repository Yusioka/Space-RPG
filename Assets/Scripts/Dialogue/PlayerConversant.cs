using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        [SerializeField] DialogueUI dialogueUI;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialogue(testDialogue);
        }
        private void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {          
            foreach (DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;
            }
        }
        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0) 
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            print(currentNode.GetText());

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            currentNode = children[randomIndex];
            onConversationUpdated();
        }
        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }
    }
}
