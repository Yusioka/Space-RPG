using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;

        private void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = stats.CalculateLevel().ToString();
        }
    }
}
