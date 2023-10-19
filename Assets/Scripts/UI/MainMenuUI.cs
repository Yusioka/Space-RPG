using GameDevTV.Utils;
using RPG.Core;
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

        LazyValue<SavingWrapper> savingWrapper;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWpapper);
        }

        private void Update()
        {
            if (savingWrapper == null) print("null");
        }

        private SavingWrapper GetSavingWpapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
