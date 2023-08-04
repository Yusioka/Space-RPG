using RPG.SceneManagement;
using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
    // Wrapper - оболочка
    public class SavingWrapper : MonoBehaviour
    {
        // const - can't change
        const string defaultSaveFile = "checkpoint";

        [SerializeField] float fadeInTime = 0.2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();

            fader.FadeOut(fadeInTime);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
