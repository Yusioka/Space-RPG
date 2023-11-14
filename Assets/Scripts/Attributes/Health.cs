using UnityEngine.Events;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Combat;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float HealthPoints { get; private set; }
        public bool Invulnerable { get; set; }

        public UnityEvent OnDie;

        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;

        bool wasDeadLastFrame = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        private void Awake()
        {
            HealthPoints = GetMaxHealthPoints();
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
            return HealthPoints <= 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (Invulnerable || HealthPoints < 0) 
                return;

            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            
            if (HealthPoints == 0)
            {
                HealthPoints = -0.01f;

                instigator.GetComponent<Fighter>().Cancel();
                AwardExperience(instigator);

                OnDie.Invoke();
            }
            else
            {
                takeDamage.Invoke(damage);
            }

            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            HealthPoints = Mathf.Min(HealthPoints + healthToRestore, GetMaxHealthPoints());
            
            UpdateState();
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetFraction()
        {
            return HealthPoints / GetMaxHealthPoints();
        }

        private void UpdateState()
        {
            var animator = GetComponent<Animator>();
            
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                GetComponent<ActionSceduler>().CancelCurrentAction();
            }
            else if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead();
        }

        private void AwardExperience(GameObject instigator)
        {          
            if (!instigator.TryGetComponent(out Experience experience)) 
                return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            var regenHealthPoints = GetMaxHealthPoints() * (regenerationPercentage / 100);
            HealthPoints = Mathf.Max(HealthPoints, regenHealthPoints);
        }

        public object CaptureState()
        {
            return HealthPoints;
        }

        public void RestoreState(object state)
        {
            HealthPoints = (float)state;
            UpdateState();
        }
    }
}