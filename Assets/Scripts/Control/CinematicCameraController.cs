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
                if (camera == null) return;
                camera.SetActive(true);
            }
        }

        public void EnablePreBossAnimation(GameObject camera)
        {
            if (portal.CanDoSomething)
            {
                if (camera == null) return;
                camera.SetActive(true);
            }
        }

        public void EnableBossAnimation(GameObject camera)
        {
            if (GameObject.FindWithTag("Boss").GetComponent<BossController>().CanEnableCamera)
            {
                if (camera == null) return;
                camera.SetActive(true);
            }
        }
    }
 
}