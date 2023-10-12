using System;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Demo Targeting", menuName = "Abilities/Targeting/Demo", order = 1)]
    public class DemoTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            throw new NotImplementedException();
        }
    }
}
