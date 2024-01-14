using RPG.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class BossController : MonoBehaviour
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        [SerializeField] int numberOfAbilities = 3;
        [SerializeField] ActionItem ability;

        float duration = 5;
        float time = 0;

        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        public ActionItem GetAction(int index)
        {
            return dockedItems[index].item;
        }

        public int GetNumber(int index)
        {
            return dockedItems[index].number;
        }

        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                bool succeeded = dockedItems[index].item.Use(user);
                return true;
            }
            return false;
        }

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (GetNumber(i) == 0)
                {
                    Use(i, this.gameObject);
                }
            }
        }

        public void AddAction(ActionItem item, int index)
        {
            var slot = new DockedItemSlot();
            slot.item = item as ActionItem;
            dockedItems[index] = slot;
        }

        private void Awake()
        {
            time = duration;
            AddAction(ability, 0);
        }

        private void Update()
        {
            UseAbilities();
        }
    }
}
