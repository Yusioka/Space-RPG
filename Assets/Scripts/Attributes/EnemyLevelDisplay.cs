using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.Abilities
{
    public class EnemyLevelDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health health = fighter.GetTargetHealth();

            if (health == null) return;

            BaseStats stats = health.gameObject.GetComponent<BaseStats>();

            if (stats == null) return;

            GetComponent<TextMeshProUGUI>().text = stats.CalculateLevel().ToString();
        }
    }
}
