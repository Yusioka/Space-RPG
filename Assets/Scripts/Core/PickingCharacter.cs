using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
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

        private void Update()
        {
            Time.timeScale = 2f;
        }
    }
}