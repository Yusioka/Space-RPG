using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialoqueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

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
        }
        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                // обновляет OnGUI()
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
                // для каждой записи в записях выбранного диалога...
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    node.text = EditorGUILayout.TextField(node.text);
                }
            }
        }
    }
}