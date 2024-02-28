using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Mana Effect", menuName = "Abilities/Effects/New Mana Effect", order = 0)]

    public class ManaEffect : EffectStrategy
    {
        [SerializeField] float manaChange;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var mana = target.GetComponent<Mana>();

                if (mana)
                {
                    mana.AddMana(manaChange);
                }
            }

            finished();
        }
    }
}
