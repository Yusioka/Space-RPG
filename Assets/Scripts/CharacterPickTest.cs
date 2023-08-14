using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.Control
{
    public class CharacterPickTest : MonoBehaviour, ISaveable
    {
        bool isMale = false;

        public bool GetIsMale()
        {
            return isMale;
        }

        public void MalePick()
        {
            isMale = true;
        }
        public void FemalePick()
        {
            isMale = false;
        }

        public void Accept()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void StartGame()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public object CaptureState()
        {
            return isMale;
        }

        public void RestoreState(object state)
        {
            isMale = (bool)state;
        }
    }
}
