using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        [SerializeField] float startingBalance = 1000f;

        float balance = 0;
        public event Action OnChanged;

        Experience experience;

        private void Awake()
        {
            balance = startingBalance;

            experience = GetComponent<Experience>();
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            OnChanged?.Invoke();
        }


        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
            OnChanged?.Invoke();
        }

        public int AddItems(InventoryItem item, int number)
        {
            if (item is ExperienceItem)
            {
                experience.GainExperience(item.GetPrice() * number);
                return number;
            }

            if (item is CurrencyItem)
            {
                UpdateBalance(item.GetPrice() * number);
                return number;
            }
            return 0;
        }
    }
}
