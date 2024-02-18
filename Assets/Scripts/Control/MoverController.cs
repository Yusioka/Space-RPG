using RPG.Core;
using RPG.Saving;
using UnityEngine;

namespace RPG.Control
{
    public class MoverController : MonoBehaviour, ISaveable
    {
        public bool IsButtonsMoving()
        {
            return FindAnyObjectByType<TypeOfControl>().IsButtonsMoving;
        }

        public object CaptureState()
        {
            return FindAnyObjectByType<TypeOfControl>().IsButtonsMoving;
        }

        public void RestoreState(object state)
        {
            FindAnyObjectByType<TypeOfControl>().IsButtonsMoving = (bool)state;
        }
    }
}
