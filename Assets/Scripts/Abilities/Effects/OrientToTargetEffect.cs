using UnityEngine;
using System;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Orient To Target Effect", menuName = "Abilities/Effects/New Orient To Target Effect", order = 0)]
    public class OrientToTargetEffect : EffectStrategy
    {
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.GetUser().transform.LookAt(data.GetTargetedPoint());
            finished();
        }
    }
}