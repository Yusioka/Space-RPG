using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class XPDisplay : MonoBehaviour
    {
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = experience.GetExperience().ToString();
        }
    }
}
