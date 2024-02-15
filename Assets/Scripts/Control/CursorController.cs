using RPG.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
            //Vector3 mousePosition = Input.mousePosition;

            //Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            //RaycastHit hit;

  //          if (Physics.Raycast(ray, out hit))
   //         {
                //if (hit.collider.CompareTag("Ground"))
                //{
                //    SetCursor(CursorType.Movement);
                //}
                //else
                //{
                //    SetCursor(CursorType.None);
                //}

                //if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Boss"))
                //{
                //    if (hit.collider.GetComponent<AIConversant>() && hit.collider.GetComponent<AIConversant>().enabled == true)
                //    {
                //        SetCursor(CursorType.Dialogue);
                //    }

                //    else
                //    {
                //        SetCursor(CursorType.Combat);
                //    }
                //}
                //if (hit.collider.CompareTag("NPC"))
                //{
                //    SetCursor(CursorType.Dialogue);
                //}
                //if (hit.collider.CompareTag("Item"))
                //{
                //    SetCursor(CursorType.UI);
                //}
     //       }

            //else
            //{
            //    SetCursor(CursorType.None);
            //}
        }

        public void SetUICursor()
        {
            CursorMapping mapping = GetCursorMapping(CursorType.UI);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
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
