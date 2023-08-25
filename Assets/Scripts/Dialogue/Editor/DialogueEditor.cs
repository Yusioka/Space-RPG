using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace RPG.Dialogue.Editor
{
    public class DialoqueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle;
        bool isDragging = false;
        DialogueNode draggingNode = null;
        Vector2 draggingOffset;

        [MenuItem("Window/Dialogue Editor")]

        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialoqueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // as = if the object ISN'T Dialogue, it WON'T WORK, else - it WILL WORK
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset (20, 20, 20, 20);
            nodeStyle.border = new RectOffset (12, 12, 12, 12);
        }
        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                // ��������� OnGUI()
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                ProcessEvents();
                // ��� ������ ������ � ������� ���������� �������...
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                // ����������
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDown && draggingNode != null)
            {
                draggingNode = null;
            }
        }

        private void OnGUINode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node: ", EditorStyles.whiteLabel);
            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueId = EditorGUILayout.TextField(node.uniqueID);

            // ���� ���� ��������� � ������...
            if (EditorGUI.EndChangeCheck())
            {
                // ��������� ������ ������ ��������
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");

                node.text = newText;
                node.uniqueID = newUniqueId;
            }

            GUILayout.EndArea();
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}