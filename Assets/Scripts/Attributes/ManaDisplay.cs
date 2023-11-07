using System.Collections;
using System.Collections.Generic;
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
            manaSlider.maxValue = mana.GetMaxMana();
            manaSlider.value = mana.GetMana();
        }
    }
}
