using RPG.Attributes;
using RPG.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class BossController : MonoBehaviour
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        [SerializeField] int numberOfAbilities = 3;
        [SerializeField] ActionItem ability;

        Health health;
        Animator animator;

        float duration = 5;
        float time = 0;

        //
        public float wanderRadius = 10f;
        public float wanderTimer = 5f;
        public float attackCooldown = 2f;

        public float knockbackForce = 10f; // Сила отталкивания

        private Transform player;
        private NavMeshAgent agent;
        private float timer;
        private bool isAttacking;

        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        public ActionItem GetAction(int index)
        {
            return dockedItems[index].item;
        }

        public int GetNumber(int index)
        {
            return dockedItems[index].number;
        }

        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                bool succeeded = dockedItems[index].item.Use(user);
                return true;
            }
            return false;
        }

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (GetNumber(i) == 0)
                {
                    Use(i, this.gameObject);
                }
            }
        }

        public void AddAction(ActionItem item, int index)
        {
            var slot = new DockedItemSlot();
            slot.item = item as ActionItem;
            dockedItems[index] = slot;
        }

        private void Awake()
        {
            time = duration;
            AddAction(ability, 0);

            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            health = GetComponent<Health>();
            //
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timer = wanderTimer;
            isAttacking = false;
        }

        private void Update()
        {
        //    animator.SetTrigger("readyToAttack");

            UseAbilities();

            //if (health.HealthPoints <= health.GetMaxHealthPoints()/2)
            //{
            //    AddNewAttack();
            //}
            //
            //timer -= Time.deltaTime;

            //if (timer <= 0f && !isAttacking)
            //{
            //    StartCoroutine(Wander());
            //    timer = wanderTimer;
            //}

            //if (health.HealthPoints <= health.GetMaxHealthPoints() / 2)
            //{
            //    AddNewAttack();
            //}
        }

        private IEnumerator Wander()
        {
            isAttacking = true;
            animator.SetTrigger("readyToAttack");

            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + directionToPlayer * 10f; // Расстояние, на которое босс будет бежать

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, agent.speed * Time.deltaTime);
        //        animator.Play("RunAttack");
                yield return null;
            }

      //      yield return new WaitForSeconds(attackCooldown);

            isAttacking = false;
        }

        private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);
            return navHit.position;
        }

        private void AddNewAttack()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }

    }
}
