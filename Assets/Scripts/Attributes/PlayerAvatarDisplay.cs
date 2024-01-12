using RPG.Core;
using UnityEngine;

namespace RPG.Attributes
{
    public class PlayerAvatarDisplay : MonoBehaviour
    {
        [SerializeField] GameObject maleAvatar;
        [SerializeField] GameObject femaleAvatar;

        private void Start()
        {
            if (GameObject.FindAnyObjectByType<PickingCharacter>().IsMale)
            {
                femaleAvatar.SetActive(false);
                maleAvatar.SetActive(true);
            }

            else
            {
                femaleAvatar.SetActive(true);
                maleAvatar.SetActive(false);
            }
        }
    }
}
