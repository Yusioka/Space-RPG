using RPG.Control;
using RPG.Saving;
using RPG.SceneManagement;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject buttonPrefab;

        public void EnableLoadUI()
        {
            SavingWrapper savingWrapper = FindAnyObjectByType<SavingWrapper>();
            if (savingWrapper == null) return;
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (string save in savingWrapper.ListSaves())
            {
                GameObject buttonInstance = Instantiate(buttonPrefab, contentRoot);
                TMP_Text textComp = buttonInstance.GetComponentInChildren<TMP_Text>();
                textComp.text = Path.GetFileNameWithoutExtension(save);
                Button button = buttonInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => savingWrapper.LoadGame(save));
            }
        }

        private IEnumerator ContinueGameRoutine(SavingWrapper savingWrapper, string save)
        {
            GetComponent<ShowHideUI>().Toggle();
            yield return new WaitForSeconds(5);
            savingWrapper.ContinueGame();
        }
    }
}