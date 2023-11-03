using UnityEngine;
using RPG.Saving;
using System;
using RPG.Inventories;

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
                GainExperience(Time.deltaTime * 100);
            }
        }

        public float GetExperience()
        {
            return experiencePoints;
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
