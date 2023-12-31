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
            BaseStats stats = fighter.GetTargetHealth().gameObject.GetComponent<BaseStats>();

            GetComponent<TextMeshProUGUI>().text = stats.CalculateLevel().ToString();
        }
    }
}
