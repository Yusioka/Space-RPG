using RPG.Combat;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class EnemyNameDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            BaseStats stats = fighter.GetTargetHealth().gameObject.GetComponent<BaseStats>();

            GetComponent<TextMeshProUGUI>().text = fighter.GetTargetHealth().gameObject.name;

          //  GetComponent<TextMeshProUGUI>().text = stats.GetCharacterClass().ToString();
        }
    }
}
