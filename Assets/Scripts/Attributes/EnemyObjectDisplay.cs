using UnityEngine.UI;
using UnityEngine;
using RPG.Combat;
using RPG.Stats;

namespace RPG.Attributes
{
    public class EnemyObjectDisplay : MonoBehaviour
    {
        [SerializeField] AvatarCharacterClass[] characterClasses = null;

        [SerializeField] GameObject enemyObject;
        [SerializeField] GameObject enemyAvatar = null;
        Fighter fighter;

        [System.Serializable]
        class AvatarCharacterClass
        {
            public CharacterClass characterClass;
            public Sprite avatar;
        }

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health target = fighter.GetTargetHealth();

            if (target == null)
            {
                enemyObject.SetActive(false);
                return;
            }

            enemyObject.SetActive(true);

            foreach (AvatarCharacterClass characterClass in characterClasses)
            {
                if (target.GetComponent<BaseStats>().GetCharacterClass() == characterClass.characterClass)
                {
                    enemyAvatar.GetComponent<Image>().sprite = characterClass.avatar;
                }
            }
        }
    }
}