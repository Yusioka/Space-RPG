using RPG.Core;
using UnityEngine;

namespace RPG.Core
{
    public class PlayRandomPlayerSound : MonoBehaviour
    {
        [SerializeField] AudioClip[] sounds;

        public void PlaySound()
        {
            if (!FindAnyObjectByType<PickingCharacter>().IsMale)
            {
                AudioClip clip = sounds[Random.Range(0, 3)];
                GetComponent<AudioSource>().PlayOneShot(clip);
            }
            else
            {
                AudioClip clip = sounds[Random.Range(3, sounds.Length)];
                GetComponent<AudioSource>().PlayOneShot(clip);
            }
        }
    }
}
