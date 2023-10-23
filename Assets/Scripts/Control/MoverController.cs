using UnityEngine;

namespace RPG.Control
{
    public class MoverController : MonoBehaviour
    {
        public bool isButtonsMoving = false;

        public bool IsButtonsMoving()
        {
            return isButtonsMoving;
        }


        public void MouseMoving()
        {
            isButtonsMoving = false;
        }
        public void ButtonsMoving()
        {
            isButtonsMoving = true;
        }
    }
}
