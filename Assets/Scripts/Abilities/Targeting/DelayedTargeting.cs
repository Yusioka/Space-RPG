using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Targeting", menuName = "Abilities/Targeting/Delayed", order = 0)]
    public class DelayedTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.StartCoroutine(Targeting(data, finished));
        }

        private IEnumerator Targeting(AbilityData data, Action finished)
        {
            if (targetingPrefabInstance == null)
            {
                targetingPrefabInstance = Instantiate(targetingPrefab);
            }

            else
            {
                targetingPrefabInstance.gameObject.SetActive(true);
            }

            targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);

            while (!data.IsCancelled())
            {
                targetingPrefabInstance.position = data.GetTargetedPoint();

                if (Input.GetMouseButtonDown(0))
                {
                    // Absorb the whole mouse click
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
               //     data.SetTargetedPoint(raycastHit.point);
              //      data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                    break;
                }

                yield return null;
            }

            targetingPrefabInstance.gameObject.SetActive(false);
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}