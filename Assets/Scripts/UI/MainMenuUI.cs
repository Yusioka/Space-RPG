using RPG.Saving;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField newGameNameField;

        SavingWrapper savingWrapper;

        private void Awake()
        {
            savingWrapper = GetSavingWpapper();
        }

        private SavingWrapper GetSavingWpapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.NewGame(newGameNameField.text);
        }
    }
}
