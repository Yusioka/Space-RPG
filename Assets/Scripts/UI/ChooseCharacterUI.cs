using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ChooseCharacterUI : MonoBehaviour
    {
        PickingCharacter pickingCharacter;
        SavingWrapper savingWrapper;

        [SerializeField] MainMenuUI mainMenuUI;

        [SerializeField] Button chooseMaleButton;
        [SerializeField] Button chooseFemaleButton;
        [SerializeField] Button saveButton;

        private void Awake()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
        }

        private void Start()
        {
            pickingCharacter = GameObject.FindAnyObjectByType<PickingCharacter>();
            chooseMaleButton.onClick.AddListener(() => pickingCharacter.ChooseMale());
            chooseFemaleButton.onClick.AddListener(() => pickingCharacter.ChooseFemale());
            saveButton.onClick.AddListener(() => SaveChanges());
        }

        public void SaveChanges()
        {
            savingWrapper.Save();
            savingWrapper.NewGame(mainMenuUI.GetNewGameNameField().text);
        }
    }
}
