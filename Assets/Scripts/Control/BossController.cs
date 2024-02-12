using RPG.Attributes;
using RPG.Inventories;
using RPGCharacterAnims.Actions;
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

        /// <summary>
        /// 
        public float castTime = 2f; // Время каста заклинания
        public float memoryTime = 1f; // Время запоминания позиции игрока
        public float attackSpeed = 50f; // Скорость атаки босса

        private Transform playerTransform; // Позиция игрока
        private Vector3 rememberedPosition; // Запомненная позиция игрока
        public bool isCasting = false; // Флаг, указывающий, кастует ли босс в данный момент
        /// </summary>
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
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
            //      UseAbilities();
            if (!isCasting)
            {
                StartCoroutine(CastSpell());
            }
        }

        public void MoveTo(Vector3 destination, float speed)
        {
            agent.destination = destination;
            agent.speed = speed;
            agent.isStopped = false;
        }

        IEnumerator CastSpell()
        {
            isCasting = true;

            rememberedPosition = playerTransform.position;
            yield return new WaitForSeconds(castTime);

            StartCoroutine(MoveThroughPosition(rememberedPosition));
            yield return new WaitForSeconds(castTime);
        }

        IEnumerator MoveThroughPosition(Vector3 targetPosition)
        {
            Vector3 currentPosition = transform.position;
            Vector3 directionToTarget = (targetPosition - currentPosition).normalized;
            Vector3 destination = targetPosition + directionToTarget * 10f;

            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                MoveTo(destination, attackSpeed);
                yield return null;
            }
            isCasting = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }

    }
}
