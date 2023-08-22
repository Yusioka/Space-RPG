using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogue.Editor
{
    public class DialoqueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]

        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialoqueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OpenDialogue(int instanceID, int line)
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

        private void OnGUI()
        {
            EditorGUI.LabelField(new Rect(50, 30, 200, 200), "Hi");
        }
    }
}