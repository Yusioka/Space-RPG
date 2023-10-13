using UnityEngine;
using System;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Abilities/Effects/New Trigger Animation Effect", order = 0)]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Animator animator = data.GetUser().GetComponent<Animator>();
            animator.SetTrigger(animationTrigger);
            finished();
        }
    }
}