using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float suspiciousTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;

        GameObject player;
        Fighter fighter;
        Health health;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (DistanceToPlayer(player) <= chaseDistance && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (DistanceToPlayer(player) > chaseDistance)
            {
                SuspicionBehaviour();
            }

            if (timeSinceLastSawPlayer > suspiciousTime)
            {
                PatrolBehaviour();
                if (DistanceToPlayer(player) <= chaseDistance)
                {
                    transform.LookAt(player.transform.position);
                }
            }
            // or 
            //if (timeSinceLastSawPlayer < suspiciousTime)
            //{
            //    GetComponent<ActionSceduler>().CancelCurrentAction();
            //}

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;

            if (player.GetComponent<Health>().IsDead())
            {
                //GetComponent<Mover>().StartMoveAction(guardPosition);
                PatrolBehaviour();
            }
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition);
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            timeSinceLastSawPlayer = Mathf.Infinity;
        //    timeSinceAggrevated = Mathf.Infinity;
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

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
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
            //    GetComponent<Mover>().StartMoveActionByMouse(nextPosition, 1f);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private float DistanceToPlayer(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        // An Unity function
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
