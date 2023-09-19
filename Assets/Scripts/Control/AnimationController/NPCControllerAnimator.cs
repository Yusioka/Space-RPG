using UnityEngine;

namespace RPG.Control.AnimationController
{
    public class NPCControllerAnimator : MonoBehaviour
    {
        [SerializeField] AnimationClip[] animations;
        Animator animator;

        public void SelectAnimation(Animations animationName)
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                foreach (AnimationClip clip in animations)
                {
                    if (clip.name == animationName.ToString())
                    {
                        animator.Play(animationName.ToString());
                    }
                }
            }
            Debug.LogError("There are no Animator in the component");
        }
    }
}
