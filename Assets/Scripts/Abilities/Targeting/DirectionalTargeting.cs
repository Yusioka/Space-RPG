using System;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Abilities/Targeting/Directional Targeting", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }
            finished();
        }
    }
}