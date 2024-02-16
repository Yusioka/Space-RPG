using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageText = null;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            transform.LookAt(2 * transform.position - Camera.main.transform.position);
        }

        public void SetValue(float amount)
        {
            damageText.text = string.Format("{0:0.#}", amount);
        }
    }
}