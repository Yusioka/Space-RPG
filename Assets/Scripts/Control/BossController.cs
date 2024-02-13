using RPG.Attributes;
using RPG.Combat;
using RPG.Inventories;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class BossController : MonoBehaviour, IMover
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        [SerializeField] ActionItem[] ability;
        [SerializeField] int wanderRadius = 200;
        [SerializeField] float totalCooldownTime = 5;
        [SerializeField] float dashCooldownTime = 6;

        Health health;
        Fighter fighter;
        Animator animator;

        Transform playerTransform;
        NavMeshAgent navMeshAgent;

        float castTime = 2f;
        float speed = 5f;

        Vector3 rememberedPosition;
        bool isCasting = false;

        float currentCooldownTime = 0;
        float currentDashCooldownTime = 0;

        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        public void UseAction(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].item.Use(user);
            }
        }

        public void AddAction(ActionItem item, int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, dockedItems[index].item))
                {
                    dockedItems[index].number ++;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item as ActionItem;
                dockedItems[index] = slot;
            }
        }

        //
        public float GetSpeed()
        {
            return speed;
        }
        //

        private void Awake()
        {
            for (int i = 0; i < ability.Length; i++)
            {
                AddAction(ability[i], i);
            }

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            fighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            animator.Play("ReadyToAttack");
        }

        private void Update()
        {
            currentCooldownTime += Time.deltaTime;
            currentDashCooldownTime += Time.deltaTime;

            if (!CanMoveTo()) return;

            fighter.Attack(playerTransform.gameObject);
            UpdateAnimator();

            AddNewAttack();
            if (currentCooldownTime >= totalCooldownTime)
            {
                UseAction(Random.Range(0, ability.Length), gameObject);
                currentCooldownTime = 0;
            }
        }

        private void AddNewAttack()
        {
            if (health.HealthPoints <= health.GetMaxHealthPoints() / 2)
            {
                if (!isCasting && currentDashCooldownTime >= dashCooldownTime)
                {
                    StartCoroutine(CastSpell());
                }
            }
        }

        private IEnumerator CastSpell()
        {
            isCasting = true;

            rememberedPosition = playerTransform.position;
            yield return new WaitForSeconds(castTime);

            StartCoroutine(MoveThroughPosition(rememberedPosition));
        }

        private IEnumerator MoveThroughPosition(Vector3 targetPosition)
        {
            Vector3 currentPosition = (transform.position - targetPosition);
            Vector3 directionToTarget = (targetPosition - currentPosition).normalized;
            Vector3 destination = targetPosition + directionToTarget * 10f;

            if (Vector3.Distance(transform.position, playerTransform.position) <= 8f)
            {
                animator.Play("RunAttack");
            }

            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                MoveTo(destination, speed);
                yield return null;
            }

            currentDashCooldownTime = 0;
            isCasting = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speedY", speed);
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = speed;
            navMeshAgent.isStopped = false;
        }

        public bool CanMoveTo()
        {
            if (health.IsDead())
            {
                return false;
            }
            return true;
        }

        public void StartMoveAction() { }
    }
}
