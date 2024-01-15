using System;
using UnityEngine;
using RPG.Attributes;
using System.Collections;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Continue Health Effect", menuName = "Abilities/Effects/New Continue Health Effect", order = 0)]
    public class ContinueHealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        [SerializeField] float duration = 1f;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();

                if (health)
                {
                    if (healthChange < 0)
                    {
                        data.StartCoroutine(ContinueDamage(data, health, -healthChange, finished));
                    }
                    else
                    {
                        data.StartCoroutine(ContinueHeal(health, healthChange, finished));
                    }
                }
            }
        }

        private IEnumerator ContinueDamage(AbilityData data, Health health, float healthChange, Action finished)
        {
            health.TakeDamage(data.GetUser(), healthChange);
            yield return new WaitForSeconds(duration);
            health.TakeDamage(data.GetUser(), healthChange);
            yield return new WaitForSeconds(duration);
            health.TakeDamage(data.GetUser(), healthChange);
            finished();
        }

        private IEnumerator ContinueHeal(Health health, float healthChange, Action finished)
        {
            health.Heal(healthChange);
            yield return new WaitForSeconds(duration);
            health.Heal(healthChange);
            yield return new WaitForSeconds(duration);
            health.Heal(healthChange);
            finished();
        }
    }
}