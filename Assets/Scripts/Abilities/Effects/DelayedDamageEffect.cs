using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Delayed Damage Effect", menuName = "Abilities/Effects/New Delayed Damage Effect", order = 0)]
    public class DelayedDamageEffect : EffectStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform prefabToSpawn;
        [SerializeField] float damage;
        [SerializeField] float damageDelay = 1;
        [SerializeField] float destroyDelay = -1;


        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            Transform instance = Instantiate(prefabToSpawn);

            instance.localScale = new Vector3(areaAffectRadius * 2, 0.01f, areaAffectRadius * 2);

            instance.position = data.GetTargetedPoint();

            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();

                if (health)
                {
                    yield return new WaitForSeconds(damageDelay);

                    foreach (var objectInRadius in GetGameObjectsInRadius(instance.position))
                    {
                        if (target != objectInRadius) continue;

                        health.TakeDamage(data.GetUser(), damage);
                    }
                }
            }

            yield return null;

            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }

            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            Collider[] colliders = Physics.OverlapSphere(point, areaAffectRadius);

            foreach (var collider in colliders)
            {
                yield return collider.gameObject;
            }
        }
    }
}