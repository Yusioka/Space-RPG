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
        [SerializeField] GameObject chooseCharacterUI;

        SavingWrapper savingWrapper;

        public TMP_InputField GetNewGameNameField()
        {
            return newGameNameField;
        }

        private void Awake()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void Update()
        {
            if (savingWrapper == null) print("null");
        }

        public void ContinueGame()
        {
            savingWrapper.ContinueGame();
        }

        public void ChooseCharacter()
        {
            chooseCharacterUI.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
