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


        [System.Serializable]
        class AvatarNPCClass
        {
            public CharacterClass characterClass;
            public Sprite avatar;
        }


        private void Update()
        {
      //      print(currentConversant);
        }

        //public Image GetAvatar()
        //{
        //    //foreach (AvatarNPCClass characterClass in characterClasses)
        //    //{
        //    //    if (currentConversant.GetComponent<BaseStats>().GetCharacterClass() == characterClass.characterClass)
        //    //    {
        //    //        npcAvatar.sprite = characterClass.avatar;
        //    //    }
        //    //}
        //    //return npcAvatar;
        //}
    }
}