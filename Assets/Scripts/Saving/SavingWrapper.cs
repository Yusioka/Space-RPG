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
        [SerializeField] KeyCode savingKey;
        [SerializeField] KeyCode loadKey;
        [SerializeField] KeyCode deleteKey;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();

         //   fader.FadeOut(fadeInTime);
        //    yield return fader.FadeIn(fadeInTime);
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
            GetComponent<SavingSystem>().Save(defaultSaveFile);
            print("saved");
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            print("loaded");
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
