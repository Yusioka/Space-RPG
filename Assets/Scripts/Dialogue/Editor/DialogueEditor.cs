using UnityEngine;
using UnityEditor;

namespace RPG.Dialogue.Editor
{
    public class DialoqueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialoqueEditor), false, "Dialogue Editor");
        }
    }
}