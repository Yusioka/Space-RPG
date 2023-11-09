using UnityEngine.Events;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Combat;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        public UnityEvent onDie;

        bool experienceGained = false;

        bool invulnerable = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        float healthPoints;
        bool wasDeadLastFrame = false;

        private void Awake()
        {
            healthPoints = GetInitialHealth();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }


        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return healthPoints <= 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (invulnerable) return;
            if (healthPoints < 0) return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                onDie.Invoke();
                instigator.GetComponent<Fighter>().Cancel();
                AwardExperience(instigator);
                healthPoints = -0.01f;
            }
            else
            {
                print(healthPoints);
                takeDamage.Invoke(damage);
            }
            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints = Mathf.Min(healthPoints + healthToRestore, GetMaxHealthPoints());
            UpdateState();
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                GetComponent<ActionSceduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }
            wasDeadLastFrame = IsDead();

        }
        private void AwardExperience(GameObject instigator)
        {
            if (experienceGained) return;
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            experienceGained = true;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public bool GetInvulnerable()
        {
            return invulnerable;
        }

        public void SetInvulnerable(bool state)
        {
            invulnerable = state;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            UpdateState();
        }
    }
}