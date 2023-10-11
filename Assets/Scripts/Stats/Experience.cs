using UnityEngine;
using RPG.Saving;
using System;

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
            if (Input.GetKeyUp(KeyCode.E))
            {
                GainExperience(Time.deltaTime * 1000);
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
