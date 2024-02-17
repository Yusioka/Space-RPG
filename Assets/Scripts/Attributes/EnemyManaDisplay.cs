using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyManaDisplay : MonoBehaviour
    {
        Mana mana;
        Slider manaSlider;
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            manaSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            if (fighter.GetTargetHealth() == null) return;

            if (mana == null || GameObject.FindWithTag("Boss") == null)
            {
                manaSlider.value = 1;
            }

            if (GameObject.FindWithTag("Boss"))
            {
                mana = GameObject.FindWithTag("Boss").GetComponent<Mana>();
            }

            manaSlider.maxValue = mana.GetMaxMana();
            manaSlider.value = mana.GetMana();
        }
    }
}
