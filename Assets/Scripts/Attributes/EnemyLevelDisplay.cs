using RPG.Combat;
using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities
{
    public class EnemyLevelDisplay : MonoBehaviour
    {
        BaseStats stats;
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
    //        stats = fighter.GetTargetHealth().GetComponent<BaseStats>();
        }

        private void Update()
        {
            print(stats.CalculateLevel().ToString());
            //  GetComponent<Text>().text = stats.CalculateLevel().ToString();
        }
    }
}
