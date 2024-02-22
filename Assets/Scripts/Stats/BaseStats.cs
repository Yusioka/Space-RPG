using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        // slider
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
      //  [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        [SerializeField] AudioSource levelUpAudio;

        public event Action onLevelUp;

        int currentLevel = 0;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }
        private void OnEnable()
        {
            if (experience != null)
            {
                // �������� ������ UpdateLevel � ������ ����� ������� ��� �������
                // (��������, ��� ����� ������) ����������� ��� ����, ����� �� ����������� ���������� ������ �������
                experience.onExperienceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
              //  LevelUpEffect();
                onLevelUp();

                levelUpAudio?.Play();

                GetComponent<Health>().Heal(10000);
            }
        }

        private void LevelUpEffect()
        {
         //   Instantiate(levelUpParticleEffect, transform);
        }

        public CharacterClass GetCharacterClass()
        {
            return characterClass;
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        // ����� ���������� �� ���������� ��������
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;                                
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
        public float GetStatByPrevLevel(Stat stat)
        {
            if (GetLevel() <= 1) return 0;
            float baseStatByPrevLevel = progression.GetStat(stat, characterClass, GetLevel() - 1);
            return (baseStatByPrevLevel + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null || experience.GetExperience() == 0) return startingLevel;

            float currentXP = GetComponent<Experience>().GetExperience();
            // penultimate - �������������
            //   ���� � ���� 5 �������, � ������� progression ��������� ���� 4, �.�. � 4 ������ ��������, �������
            //     XP ��� ���� ��� ���������� 5-��
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
