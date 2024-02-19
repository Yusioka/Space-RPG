using RPG.Dialogue;
using UnityEngine;

namespace RPG.Control
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] CursorMapping[] cursorMappings = null;

        [System.Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    SetCursor(CursorType.Movement);
                }
                else
                {
                    SetCursor(CursorType.None);
                }

                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Boss") || hit.collider.CompareTag("BossSon"))
                {
                    if (hit.collider.GetComponent<AIConversant>() && hit.collider.GetComponent<AIConversant>().enabled == true)
                    {
                        SetCursor(CursorType.Dialogue);
                    }

                    else
                    {
                        SetCursor(CursorType.Combat);
                    }
                }
                if (hit.collider.CompareTag("NPC"))
                {
                    SetCursor(CursorType.Dialogue);
                }
                if (hit.collider.CompareTag("Item"))
                {
                    SetCursor(CursorType.Pickup);
                }
            }

            else
            {
                SetCursor(CursorType.None);
            }
        }

        public void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type) return mapping;
            }
            return cursorMappings[0];
        }
    }
}
