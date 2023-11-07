using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class EnemyManaDisplay : MonoBehaviour
    {
        Slider manaSlider;

        private void Start()
        {
            manaSlider = GetComponent<Slider>();
            manaSlider.value = 1;
        }
    }
}
