using RPG.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        //private void Update()
        //{
        //    if (fighter.GetHealth() == null)
        //    {
        //        GetComponent<Text>().text = "N/A";
        //        return;
        //    }
        //    Health health = fighter.GetHealth();
        //    GetComponent<Text>().text = String.Format("{0}%", health.GetHealthPercentage());
        //}
    }
}
