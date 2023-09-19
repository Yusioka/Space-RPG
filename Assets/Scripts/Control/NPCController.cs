using RPG.Control.AnimationController;
using RPG.Core;
using RPG.Movement;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] Animations animationName;
        [SerializeField] float chaseDistance = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float patrolSpeed = 0.2f;
        [SerializeField] float lookAtOffset = 0.15f;

        GameObject player;

        Animator animator;
        Quaternion startRotation;
        Vector3 guardPosition;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            startRotation = transform.rotation;
            guardPosition = transform.position;
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.Play(animationName.ToString());
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition);
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (DistanceToPlayer(player) <= chaseDistance)
            {
                LookAtPlayer();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    animator.Play("Idle");
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            else
            {
                transform.rotation = startRotation;
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                animator.Play(animationName.ToString());
                GetComponent<Mover>().StartMoveActionByMouse(nextPosition, patrolSpeed);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private float DistanceToPlayer(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void LookAtPlayer()
        {
            timeSinceArrivedAtWaypoint = 0;

            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;

            // Поворачиваем НПС к игроку
            if (directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
                animator.Play("Idle");
            }

            GetComponent<ActionSceduler>().CancelCurrentAction();
        }
    }
}
