using RPG.Stats;
using System;
using UnityEngine;
using UnityEngine.UI;

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
            GetComponent<Text>().text = stats.CalculateLevel().ToString();
        }
    }
}
