using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
