using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "FX Effect", menuName = "Abilities/Effects/New FX Effect", order = 0)]
    public class SpawnFXEffect : EffectStrategy
    {
        [SerializeField] Transform prefabToSpawn;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                Transform instance = Instantiate(prefabToSpawn, target.transform);
                instance.position = data.GetTargetedPoint();

                if (destroyDelay > 0)
                {
                    yield return new WaitForSeconds(destroyDelay);
                    Destroy(instance.gameObject);
                }
            }

            finished();
        }
    }
}