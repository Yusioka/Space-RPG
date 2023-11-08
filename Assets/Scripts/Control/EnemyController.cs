using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

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
            player = GameObject.FindGameObjectWithTag("Player");
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

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            print("updating");
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speedY", speed);
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
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            AggrevateNearbyEnemies();
        }

        //
        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                EnemyController ai = hit.collider.GetComponent<EnemyController>();
                if (ai == null) continue;

                //if (health.GetMaxHealthPoints() > 14000)
                //{
                //    ai.Aggrevate();
                //}

                if (firstTimeAggrevated)
                {
                    ai.Aggrevate();
                }
                else if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    ai.Aggrevate();
                }
            }
            firstTimeAggrevated = false;
        }
        //

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
