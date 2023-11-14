using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField;

        Purse playerPurse = null;

        private void OnEnable()
        {           
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out playerPurse))
            {
                playerPurse.OnChanged += RefreshUI;
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            balanceField.text = $"$ {playerPurse.GetBalance()}";
        }
    }
}
