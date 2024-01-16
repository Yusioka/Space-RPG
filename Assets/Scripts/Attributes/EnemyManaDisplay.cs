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
            if (GameObject.FindWithTag("Boss") == null) return;

            if (GameObject.FindWithTag("Boss") == fighter.GetTargetHealth().gameObject)
            {
                mana = GameObject.FindWithTag("Boss").GetComponent<Mana>();
            }

            if (mana == null)
            {
                manaSlider.value = 1;
                return;
            }

            manaSlider.maxValue = mana.GetMaxMana();
            print(mana.GetMana());
            manaSlider.value = mana.GetMana();
        }
    }
}
