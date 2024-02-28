using RPG.Combat;
using System;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Active Target Targeting", menuName = "Abilities/Targeting/Active Target Targeting", order = 0)]
    public class ActiveTargetTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[] { data.GetUser().GetComponent<Fighter>().GetTargetHealth().gameObject });
            data.SetTargetedPoint(data.GetUser().GetComponent<Fighter>().GetTargetHealth().transform.position);
            finished();
        }
    }
}
