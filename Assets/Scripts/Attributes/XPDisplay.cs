using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience;
        Slider XpSlider;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            XpSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            XpSlider.maxValue = experience.CalculateMaxExperienceToNextLevel();
            XpSlider.value = experience.GetCurrentLevelExperience();
        }
    }
}
