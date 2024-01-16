using RPG.Combat;
using System;
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
            //if (fighter.GetTargetHealth() == null)
            //{
            //    GetComponent<Text>().text = "N/A";
            //    return;
            //}

            Health health = fighter.GetTargetHealth();

            if (health == null) return;

            healthSlider.maxValue = health.GetMaxHealthPoints();
            healthSlider.value = health.HealthPoints;
        }
    }
}
