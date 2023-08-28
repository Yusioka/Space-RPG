using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] 
        List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                // ������ nodeLookup["a"] = ���� � ����� uniqueID;
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }
        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.children)
            {
                // ��������� �� ������� child-����� � Dictionary
                if (nodeLookup.ContainsKey(childID))
                {
                    // ��������� ��� � uniqueID � �������� ����� childID ������ ����
                    yield return nodeLookup[childID];
                }
            }
        }
    }
}
