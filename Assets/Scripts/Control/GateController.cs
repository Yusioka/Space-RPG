using RPG.Core;
using RPG.Quests;
using RPG.SceneManagement;
using System.Collections;
using UnityEngine;

namespace RPG.Control
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] Condition condition;
        [SerializeField] Portal portalNearGates;

        QuestList playerQuestList;


    //    private void Awake()
    //    {
    //        var player = GameObject.FindGameObjectWithTag("Player");
    //        playerQuestList = player.GetComponent<QuestList>();
    //    }

    //    private void Update()
    //    {
    //        StartCoroutine(CloseGates());
    //    }

    //    private bool CanGetAnimation(QuestList questList)
    //    {
    //        return condition.Check(questList.GetComponents<IPredicateEvaluator>());
    //    }

    //    private void OnTriggerEnter(Collider other)
    //    {
    //        if (other.gameObject.tag == "Player")
    //        {

    //            if (CanGetAnimation(playerQuestList))
    //            {
    //                GetComponent<Animator>().SetTrigger("open");
    //            }
    //        }
    //    }

    //    private IEnumerator CloseGates()
    //    {
    //        if (portalNearGates.MovedThroughPortal)
    //        {
    //            yield return new WaitForSeconds(2);
    //            GetComponent<Animator>().SetBool("isCloseIdle", true);
    //        }
    //    }
    }
}
