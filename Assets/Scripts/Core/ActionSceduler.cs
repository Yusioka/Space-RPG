using UnityEngine;

namespace RPG.Core
{
    public class ActionSceduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction (IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }
        public void CancelCurrentAction()
        {
            currentAction = null;
        }
    }
}
