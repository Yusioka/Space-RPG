using GameDevTV.Utils;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        float mana;

        private void Awake()
        {
            mana = GetMaxMana();
        }

        private void Update()
        {
            if (mana < GetMaxMana())
            {
                mana += GetManaRegenRate() * Time.deltaTime;
                if (mana > GetMaxMana())
                {
                    mana = GetMaxMana();
                }
            }
        }

        public float GetMana()
        {
            return mana;
        }
        public void AddMana(float mana)
        {
            this.mana += mana;
        }

        public float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetManaRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana)
            {
                return false;
            }
            mana -= manaToUse;
            return true;
        }

        public object CaptureState()
        {
            return mana;
        }

        public void RestoreState(object state)
        {
            mana = (float)state;
        }
    }
}