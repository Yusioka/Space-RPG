using RPG.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    // Wrapper - оболочка
    public class SavingWrapper : MonoBehaviour
    {
        // const - can't change
        const string currentSaveKey = "currentSaveName";

        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] KeyCode savingKey;
        [SerializeField] KeyCode loadKey;
        [SerializeField] KeyCode deleteKey;

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;

            StartCoroutine(LoadLastScene());
        }
        public void NewGame(string saveFile)
        {
            if (String.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }
        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadLastScene());
        }
        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }
        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(0);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }
        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(1);
            Save();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyUp(savingKey))
            {
                Save();
            }
            if (Input.GetKeyUp(loadKey))
            {
                Load();
            }
            if (Input.GetKeyUp(deleteKey))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
            print("saved");
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
            print("loaded");
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
            print("deleted");
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
