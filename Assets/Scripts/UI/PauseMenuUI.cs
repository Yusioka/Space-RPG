using UnityEngine;
using RPG.Control;
using UnityEngine.UI;
using RPG.Core;
using RPG.Saving;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] Button controlButton;
        [SerializeField] float timeScale = 1;

        PlayerController playerController;
        TypeOfControl typeOfControl;

        private void Start()
        {
            typeOfControl = GameObject.FindAnyObjectByType<TypeOfControl>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            controlButton.onClick.AddListener(() => typeOfControl.ChooseTypeOfMoving());
        }

        private void OnEnable()
        {
            if (playerController == null) return;
            Time.timeScale = 0;
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            if (playerController == null) return;
            Time.timeScale = timeScale;
            playerController.enabled = true;
        }

        public void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
    }
}