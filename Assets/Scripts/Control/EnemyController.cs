using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyController : MonoBehaviour, IMover, IAIController
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float maxSpeed = 5f;

        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float suspiciousTime = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;

        GameObject player;
        Fighter fighter;
        Health health;

        Vector3 guardPosition;
        NavMeshAgent navMeshAgent;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            navMeshAgent = new NavMeshAgent();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (CanMoveTo())
            {
                if (DistanceToObject(player) <= chaseDistance && fighter.CanAttack(player))
                {
                    timeSinceLastSawPlayer = 0;
                    AttackBehaviour();
                }
                else if (DistanceToObject(player) > chaseDistance)
                {
                    SuspicionBehaviour();
                }

                if (timeSinceLastSawPlayer > suspiciousTime)
                {
                    StartMoveAction();
                    if (DistanceToObject(player) <= chaseDistance)
                    {
                        transform.LookAt(player.transform.position);
                    }
                }

                timeSinceLastSawPlayer += Time.deltaTime;
                timeSinceArrivedAtWaypoint += Time.deltaTime;

                if (player.GetComponent<Health>().IsDead())
                {
                    MoveTo(guardPosition, maxSpeed);
                    StartMoveAction();
                }
            }
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition);
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            timeSinceLastSawPlayer = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private void SuspicionBehaviour()
        {
            fighter.Cancel();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        public void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (CanMoveTo())
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            // enemy arrives to the point and waiting for the statement
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                MoveTo(nextPosition, maxSpeed);
            }
        }

        private Vector3 GetCurrentWaypoint() => patrolPath.GetWaypoint(currentWaypointIndex);

        private void CycleWaypoint() => currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);


        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private float DistanceToObject(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        // An Unity function
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        public bool CanMoveTo()
        {
            if (health.IsDead())
            {
                return false;
            }
            return true;
        }

        public void StartMoveAction()
        {
            PatrolBehaviour();
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }
    }
}
