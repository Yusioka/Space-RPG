using RPG.Inventories;
using RPG.Attributes;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability", order = 1)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
       // [SerializeField] FilterStrategy[] filterStrategies;
       // [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0;
        [SerializeField] float manaCost = 0;

        public override bool Use(GameObject user)
        {
        //    Mana mana = user.GetComponent<Mana>();
        //    if (mana.GetMana() < manaCost) return false;

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) return false;

            AbilityData data = new AbilityData(user);

            ActionSceduler actionScheduler = user.GetComponent<ActionSceduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
            return true;
        }

        private void TargetAquired(AbilityData data)
        {
            if (data.IsCancelled()) return;

        //    Mana mana = data.GetUser().GetComponent<Mana>();
        //    if (!mana.UseMana(manaCost)) return;

            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);
            //foreach (var filterStrategy in filterStrategies)
            //{
            //    data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            //}

            //foreach (var effect in effectStrategies)
            //{
            //    effect.StartEffect(data, EffectFinished);
            //}
        }

        private void EffectFinished()
        {

        }
    }
}