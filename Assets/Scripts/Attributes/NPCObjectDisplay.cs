using UnityEngine.UI;
using UnityEngine;
using RPG.Combat;
using RPG.Stats;
using RPG.Dialogue;

namespace RPG.Attributes
{
    public class NPCObjectDisplay : MonoBehaviour
    {
        [SerializeField] AvatarNPCClass[] characterClasses = null;

        AIConversant currentConversant;
        Image npcAvatar = null;
        Fighter fighter;

        [System.Serializable]
        class AvatarNPCClass
        {
            public CharacterClass characterClass;
            public Sprite avatar;
        }

        private void Start()
        {
            currentConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>().GetCurrentConversant();
        }

        public Image GetAvatar()
        {
            foreach (AvatarNPCClass characterClass in characterClasses)
            {
                if (currentConversant.GetComponent<BaseStats>().GetCharacterClass() == characterClass.characterClass)
                {
                    npcAvatar.sprite = characterClass.avatar;
                }
            }
            return npcAvatar;
        }
    }
}