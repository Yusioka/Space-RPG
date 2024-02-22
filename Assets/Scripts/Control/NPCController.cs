using RPG.Control.AnimationController;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class NPCController : MonoBehaviour, IAIController, IMover
    {
        [SerializeField] Animations animationName;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float chaseDistance = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float patrolSpeed = 0.2f;

        GameObject player;

        Animator animator;
        NavMeshAgent navMeshAgent;
        Quaternion startRotation;
        Vector3 guardPosition;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            navMeshAgent = GetComponent<NavMeshAgent>();
            startRotation = transform.rotation;
            guardPosition = transform.position;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.Play(animationName.ToString());
            navMeshAgent.stoppingDistance = waypointTolerance;
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition);
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private void Update()
        {
            if (CanMoveTo())
            {
                if (player != null && DistanceToPlayer(player) <= chaseDistance)
                {
                    LookAtPlayer();
                }
                else
                {
                    StartMoveAction();
                }
                timeSinceArrivedAtWaypoint += Time.deltaTime;
            }
        }

        public void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint() && waypointDwellTime != 0)
                {
                    animator.Play("Idle");
                    navMeshAgent.velocity = Vector3.zero;
                    navMeshAgent.isStopped = true;
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                else if (AtWaypoint() && waypointDwellTime == 0)
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                else
                {
                    navMeshAgent.isStopped = false;
                    nextPosition = GetCurrentWaypoint();
                }
            }
            else
            {
                transform.rotation = startRotation;
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                animator.Play(animationName.ToString());
                if (gameObject.GetComponent<NavMeshAgent>().enabled)
                {
                    MoveTo(nextPosition, patrolSpeed);
                }
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
            if (chaseDistance != 0 && directionToPlayer != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
                animator.Play("Idle");
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.isStopped = true;
            }

            GetComponent<ActionSceduler>().CancelCurrentAction();
        }

        public bool CanMoveTo()
        {
            return true;
        }

        public void StartMoveAction()
        {
            PatrolBehaviour();
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = patrolSpeed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }
    }
}
