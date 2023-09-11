using TMPro;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        public void Setup(Quest quest)
        {
            title.text = quest.GetTitle();
            progress.text = "0/" + quest.GetObjectiveCount();
        }
    }
}
