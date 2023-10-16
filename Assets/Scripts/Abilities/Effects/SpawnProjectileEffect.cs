using System;
using UnityEngine;
using RPG.Attributes;
using RPG.Combat;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Targeting/Spawn Projectile Effect", order = 0)]
    public class SpawnProjctileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRighthand = true;
        [SerializeField] bool useTargetPoint = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRighthand).position;
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTarget(data, spawnPosition);
            }
            finished();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.GetUser(), data.GetTargetedPoint(), damage);
        }

        private void SpawnProjectilesForTarget(AbilityData data, Vector3 spawnPosition)
        {
            foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(data.GetUser(), health, damage);
                }
            }
        }
    }
}