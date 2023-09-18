using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

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
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (fighter != null || fighter.enabled)
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
                    //mover.StartMoveAction(guardPosition);
                    PatrolBehaviour();
                }
            }
            else
            {
                PatrolBehaviour();
            }
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

            if (patrolPath!= null)
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
                mover.StartMoveActionByMouse(nextPosition);
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
