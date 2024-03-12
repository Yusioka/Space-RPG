using RPG.Saving;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField newGameNameField;
        [SerializeField] GameObject chooseCharacterUI;

        SavingWrapper savingWrapper;

        public string GetNewGameName()
        {
            return newGameNameField.text;
        }

        private void Start()
        {
            savingWrapper = FindAnyObjectByType<SavingWrapper>();
        }

        private void Update()
        {
            if (savingWrapper == null) print("null");
        }

        public void NewGame()
        {
            savingWrapper.NewGame(GetNewGameName());
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
