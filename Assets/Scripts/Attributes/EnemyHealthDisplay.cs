using RPG.Combat;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Slider healthSlider;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            Health health = fighter.GetTargetHealth();

            if (health == null) return;

            GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", health.HealthPoints, health.GetMaxHealthPoints());
           
            healthSlider.maxValue = health.GetMaxHealthPoints();
            healthSlider.value = health.HealthPoints;
        }
    }
}
