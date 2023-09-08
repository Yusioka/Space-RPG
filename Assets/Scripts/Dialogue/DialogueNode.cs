using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] bool isPlayerSpeaking = false;
        [SerializeField] string text;
        [SerializeField] List<string> children = new List<string>();
        [SerializeField] Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;

        public bool IsPlayerSpeaking() { return isPlayerSpeaking; }
        public Rect GetRect() { return rect; }
        public string GetText() { return text; }
        public List<string> GetChildren() { return children; }
        public string GetOnEnterAction() { return onEnterAction; }
        public string GetOnExitAction() {  return onExitAction; }


#if UNITY_EDITOR
        public void SetIsPlayerSpeaking(bool isPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            this.isPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
        public void SetPosition(Vector2 newPosition)
        { 
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition; 
            EditorUtility.SetDirty(this);
        }
        public void SetText(string newText)
        {
            // позволяет делать отмену действий
            Undo.RecordObject(this, "Update Dialogue Text");
            if (newText != text)
            {
                text = newText;
            }
            EditorUtility.SetDirty(this);
        }
        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }
        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Delete Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
