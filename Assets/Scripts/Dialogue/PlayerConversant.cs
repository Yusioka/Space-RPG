using RPG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
        [SerializeField] Sprite playerAvatar;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public AIConversant GetConversant()
        {
            return currentConversant;
        }

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
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
        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        public Sprite GetCurrentConversantAvatar()
        {
            if (isChoosing)
            {
                return playerAvatar;
            }
            else
            {
                return currentConversant.GetAvatar();
            }
        }

        public IEnumerable<DialogueNode> GetChoices()
        {          
            foreach (DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    // yield - ��� break, ������ � ������������ ���������
                    yield return node;
                }
            }
        }
        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            // �-�� ������� ������
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0) 
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            // �-�� ������� ��
            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerEnterAction();
            currentNode = children[randomIndex];
            TriggerExitAction();
            onConversationUpdated();
        }
        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        public IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    // yield - ��� break, ������ � ������������ ���������
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            onConversationUpdated();
            currentNode = null;
            isChoosing = false;
            currentConversant = null;
        }

        public void TriggerEnterAction()
        {
            if (currentDialogue != null && currentNode.GetOnEnterAction() != "")
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }
        public void TriggerExitAction()
        {
            if (currentDialogue != null && currentNode.GetOnExitAction() != "")
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        void TriggerAction(string action)
        {
            if (action == "") return;
            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }
}
