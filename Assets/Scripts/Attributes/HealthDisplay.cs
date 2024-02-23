using TMPro;
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
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", health.HealthPoints, health.GetMaxHealthPoints());
            healthSlider.maxValue = health.GetMaxHealthPoints();
            healthSlider.value = health.HealthPoints;
        }
    }
}
