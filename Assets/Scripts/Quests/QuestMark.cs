using UnityEngine;

namespace RPG.Quests
{
    public class QuestMark : MonoBehaviour
    {
        [SerializeField] bool activate = true;
        [SerializeField] Quest quest;

        QuestList questList;

        private void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            activate = !questList.HasQuest(quest);
            gameObject.SetActive(activate);
        }
    }
}