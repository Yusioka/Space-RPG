using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class PickingCharacter : MonoBehaviour, ISaveable
    {
        public bool IsMale { get; set; }

        public void ChooseMale()
        {
            IsMale = true;
        }

        public void ChooseFemale()
        {
            IsMale = false;
        }

        public object CaptureState()
        {
            return IsMale;
        }

        public void RestoreState(object state)
        {
            IsMale = (bool)state;
        }
    }
}