using RPG.Core;
using RPG.Quests;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.Control
{
    public class CinematicCameraController : MonoBehaviour
    {
        [SerializeField] Portal portal;
        [SerializeField] Condition condition;


        public void EnableStartCinematic(GameObject camera)
        {
            QuestList playerQuestList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            if (condition.Check(playerQuestList.GetComponents<IPredicateEvaluator>()))
            {
                camera.SetActive(true);
            }
        }

        public void EnablePreBossAnimation(GameObject camera)
        {
            if (portal.CanDoSomething)
            {
                camera.SetActive(true);
            }
        }

        public void EnableBossAnimation(GameObject camera)
        {
            if (GameObject.FindWithTag("Boss").GetComponent<BossController>().CanEnableCamera)
            {
                print("canAttack");
                camera.SetActive(true);
            }
        }
    }
 
}