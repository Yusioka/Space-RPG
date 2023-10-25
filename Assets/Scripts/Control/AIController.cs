using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, IMover, IAction
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] float patrolMaxSpeed = 0.2f;

        Animator animator;
        NavMeshAgent navMeshAgent;
        Vector3 guardPosition;
        Vector3 nextPosition;
        bool canMove = false;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            guardPosition = transform.position;
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public virtual void PatrolBehaviour()
        {
            nextPosition = guardPosition;

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
                StartMoveAction();
            }
        }
        protected void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        protected bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }
        protected Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        public virtual void ResetingAI()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition);
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speedY", speed);
        }

        public bool CanMoveTo()
        {
            if (patrolPath != null)
            {
                return true;
            }
            return false;
        }

        public void StartMoveAction()
        {
            GetComponent<ActionSceduler>().StartAction(this);
            MoveTo(nextPosition, patrolMaxSpeed);
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = patrolMaxSpeed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            navMeshAgent.Stop();
        }
    }
}
