using RPG.Quests;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class ConditionController : MonoBehaviour
    {
        [SerializeField] BoxCollider objectToControl;
        [SerializeField] Condition condition;

        QuestList playerQuestList;
        int currentSceneIndex;

    //    private void Awake()
    //    {
    //        var player = GameObject.FindGameObjectWithTag("Player");
    //        playerQuestList = player.GetComponent<QuestList>();
    //    }

    //    private bool CanEnableObject(QuestList questList)
    //    {
    //        return condition.Check(questList.GetComponents<IPredicateEvaluator>());
    //    }

    //    private void Start()
    //    {
    //        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    }

    //    private void Update()
    //    {
    //        EnableObject(objectToControl);
    //    }

    //    private void EnableObject(BoxCollider collider)
    //    {
    //        if (CanEnableObject(playerQuestList))
    //        {
    //            collider.enabled = true;
    //        }
    //        else
    //        {
    //            collider.enabled = false;
    //        }
    //    }
    }
}
