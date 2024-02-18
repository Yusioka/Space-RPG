using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class TypeOfControl : MonoBehaviour, ISaveable
    {
        public bool IsButtonsMoving {  get; set; }

        public void ChooseTypeOfMoving()
        {
            IsButtonsMoving = !IsButtonsMoving;
        }

        public object CaptureState()
        {
            return IsButtonsMoving;
        }

        public void RestoreState(object state)
        {
            IsButtonsMoving = (bool)state;
        }
    }
}
