using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using RPG.SceneManagement;
using RPG.Attributes;
using RPG.Saving;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnlocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = 0.2f;
        [SerializeField] float healthRegenPercentage = 50;
        [SerializeField] float enemyHealthRegenPercentage = 20;

        private void Awake()
        {
            GetComponent<Health>().OnDie.AddListener(Respawn);
        }

        private void Start()
        {
            if (GetComponent<Health>().IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
         //   savingWrapper.SetLoadingScreen(fader);
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);
        }

        private void ResetEnemies()
        {
            foreach (EnemyController enemyController in FindObjectsOfType<EnemyController>())
            {
                Health health = enemyController.GetComponent<Health>();
                if (health && !health.IsDead())
                {
                    enemyController.Reset();
                    health.Heal(health.GetMaxHealthPoints() * enemyHealthRegenPercentage / 100);
                }
            }
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = respawnlocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(respawnlocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
        }
    }
}