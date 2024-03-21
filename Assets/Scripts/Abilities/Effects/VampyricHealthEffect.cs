using System;
using UnityEngine;
using RPG.Attributes;
using System.Collections;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Vampyric Health Effect", menuName = "Abilities/Effects/New Vampyric Health Effect", order = 0)]
    public class VampyricHealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        [SerializeField] float duration = 1f;

        public override void StartEffect(AbilityData data, Action finished)
        {
            var userHealth = data.GetUser().GetComponent<Health>();

            foreach (var target in data.GetTargets())
            {
                var enemyHealth = target.GetComponent<Health>();

                if (enemyHealth)
                {
                    data.StartCoroutine(ContinueDamage(data, enemyHealth, healthChange, finished));
                    data.StartCoroutine(ContinueHeal(userHealth, healthChange, finished));
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