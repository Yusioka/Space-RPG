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
        [SerializeField] float maxSpeed = 2f;
        [SerializeField] float walkingSpeed = 0.6f;

        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float suspiciousTime = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;

        [SerializeField] float shootDistance = 5f;
        [SerializeField] float aggroTime = 5f;

        GameObject player;
        Fighter fighter;
        Fighter playerFighter;
        Health health;

        Vector3 guardPosition;
        NavMeshAgent navMeshAgent;
        Quaternion startRotation;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        float timeSinceAggrevated = Mathf.Infinity;
        bool firstTimeAggrevated = true;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerFighter = player.GetComponent<Fighter>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        private void Start()
        {
            guardPosition = transform.position;
            startRotation = transform.rotation;
        }

        public float GetSpeed()
        {
            return maxSpeed;
        }

        private void Update()
        {
            if (CanMoveTo())
            {
                if (fighter.CanAttack(player) && DistanceToObject(player) < chaseDistance || (health.HealthPoints != health.GetMaxHealthPoints() && health == playerFighter.GetTargetHealth()))
                {
                    if (health.IsDead()) return;
                    timeSinceLastSawPlayer = 0;
                    AttackBehaviour();
                }
                else if (DistanceToObject(player) > chaseDistance)
                {
                    SuspicionBehaviour();
                }

                if (timeSinceLastSawPlayer > suspiciousTime)
                {
                    if (patrolPath != null)
                    {
                        StartMoveAction();
                    }

                    if (DistanceToObject(player) <= chaseDistance)
                    {
                        transform.LookAt(player.transform.position);
                    }
                }
                if (IsAggrevated())
                {
                    AttackBehaviour();
                }

                timeSinceLastSawPlayer += Time.deltaTime;
                timeSinceArrivedAtWaypoint += Time.deltaTime;
                //
                timeSinceAggrevated += Time.deltaTime;
                //

                if (player.GetComponent<Health>().IsDead())
                {
                    health.RegenerateHealth();
                    MoveTo(guardPosition, maxSpeed);
                    StartMoveAction();
                }
                UpdateAnimator();
            }
        }

        public void Aggrevate()
        {
            IsAggrevated();
            timeSinceAggrevated = 0;
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggroTime;
        }
        //

        private void UpdateAnimator()
        {
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


            timeSinceAggrevated = Mathf.Infinity;
        }

        private void SuspicionBehaviour()
        {
            health.RegenerateHealth();
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
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shootDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                EnemyController ai = hit.collider.GetComponent<EnemyController>();
                if (ai == null) continue;

                if (health.GetMaxHealthPoints() > 14000)
                {
                    ai.Aggrevate();
                }

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
            HealEnemy();
            Vector3 nextPosition = guardPosition;
            navMeshAgent.speed = walkingSpeed;

            if (patrolPath != null)
            {
                if (AtWaypoint() && waypointDwellTime != 0)
                {
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
                MoveTo(guardPosition, walkingSpeed);
                transform.rotation = startRotation;
            }

            // enemy arrives to the point and waiting for the statement
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                if (gameObject.GetComponent<NavMeshAgent>().enabled)
                {
                    firstTimeAggrevated = true;
                    MoveTo(nextPosition, walkingSpeed);
                }
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

        private float DistanceToObject(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void HealEnemy()
        {
            if (health.HealthPoints != health.GetMaxHealthPoints())
            {
                health.Heal(5);
            }
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
            navMeshAgent.speed = speed;
            navMeshAgent.isStopped = false;
        }
    }
}
