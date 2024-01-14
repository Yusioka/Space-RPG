using System;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Player Targeting", menuName = "Abilities/Targeting/Player Targeting", order = 0)]
    public class PlayerTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[] { data.GetPlayer() });
            data.SetTargetedPoint(data.GetPlayer().transform.position);
            finished();
        }
    }
}