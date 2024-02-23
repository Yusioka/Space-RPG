using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ManaDisplay : MonoBehaviour
    {
        Mana mana;
        Slider manaSlider;

        private void Awake()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
            manaSlider = GetComponent<Slider>();
        }

        private void Update()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
            manaSlider.maxValue = mana.GetMaxMana();
            manaSlider.value = mana.GetMana();
        }
    }
}
