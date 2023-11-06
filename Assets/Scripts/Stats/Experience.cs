using UnityEngine;
using RPG.Saving;
using System;
using RPG.Inventories;
using static Cinemachine.DocumentationSortingAttribute;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        // the same as 
        //public delegate void ExperienceGainedDelegate();
        // public event ......;
        public event Action onExperienceGained;


        private void Update()
        {
            if (Input.GetKey(KeyCode.O))
            {
                GainExperience(Time.deltaTime * 500);
            }
        }

        public float GetExperience()
        {
            return experiencePoints;
        }

        public float GetCurrentLevelExperience()
        {
            return (GetMaxExperiencePrevLevel() - GetExperience()) * -1;
        }

        public float GetMaxExperiencePrevLevel()
        {
            return GetComponent<BaseStats>().GetStatByPrevLevel(Stat.ExperienceToLevelUp);
        }

        public float GetMaxExperience()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp);
        }

        public float CalculateMaxExperienceToNextLevel()
        {
            return GetMaxExperience() - GetMaxExperiencePrevLevel();
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            // Вызывает ивент, когда срабатывает метод
            onExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
