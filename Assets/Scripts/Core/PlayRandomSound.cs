using UnityEngine;

namespace RPG.Core
{
    public class PlayRandomSound : MonoBehaviour
    {
        [SerializeField] AudioClip[] sounds;

        public void PlaySound()
        {
            AudioClip clip = sounds[Random.Range(0, sounds.Length)];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}