using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class CharacterPrefabController : MonoBehaviour, ISaveable
    {
        public bool ChosenMale()
        {
            return FindAnyObjectByType<PickingCharacter>().IsMale;
        }

        public object CaptureState()
        {
            return FindAnyObjectByType<PickingCharacter>().IsMale;
        }

        public void RestoreState(object state)
        {
            FindAnyObjectByType<PickingCharacter>().IsMale = (bool)state;
        }
    }
}