using RPG.Stats;
using System;
using TMPro;
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
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", experience.GetCurrentLevelExperience(), experience.CalculateMaxExperienceToNextLevel());
            XpSlider.maxValue = experience.CalculateMaxExperienceToNextLevel();
            XpSlider.value = experience.GetCurrentLevelExperience();
        }
    }
}
