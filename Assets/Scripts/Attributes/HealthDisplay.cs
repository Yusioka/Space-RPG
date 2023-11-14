using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Slider healthSlider;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            healthSlider.maxValue = health.GetMaxHealthPoints();
            healthSlider.value = health.HealthPoints;
        }
    }
}
