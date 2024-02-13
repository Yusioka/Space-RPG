using UnityEngine;

namespace RPG.Movement
{
    public interface IMover
    {
        bool CanMoveTo();
        void StartMoveAction();
        void MoveTo(Vector3 destination, float speed);
    }
}
