using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience;
        Image image;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            image = GetComponent<Image>();
        }

        private void Update()
        {
            //    GetComponent<Text>().text = (experience.GetExperience() / experience.CalculateMaxExperienceToNextLevel()).ToString();
              image.fillAmount = experience.GetCurrentLevelExperience() / experience.CalculateMaxExperienceToNextLevel();
            //  print(experienceToFill);
            //     GetComponent<Text>().text = experience.GetExperience().ToString();
        }
    }
}
