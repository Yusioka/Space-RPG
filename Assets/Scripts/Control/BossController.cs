using RPG.Attributes;
using RPG.Combat;
using RPG.Dialogue;
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

        [SerializeField] GameObject transformCinematicTarget;

        [SerializeField] RPG.Dialogue.Dialogue secondDialogue;
        [SerializeField] ActionItem[] ability;
        [SerializeField] int wanderRadius = 200;
        [SerializeField] float totalCooldownTime = 5;
        [SerializeField] float dashCooldownTime = 6;

        Health health;
        Fighter fighter;
        Animator animator;

        Transform playerTransform;
        NavMeshAgent navMeshAgent;

        bool shouldJump = false;
        bool isFalling = false;

        float castTime = 2f;
        float speed = 5f;

        Vector3 rememberedPosition;
        bool isCasting = false;

        float currentCooldownTime = 0;
        float currentDashCooldownTime = 0;

        int killedSons = 0;

        bool isSecondDialogue = false;
        bool startAttack = false;
        bool enableAttackBehaviour = false;
        bool attackBehaviourEnabled;

        public bool EnableCamera { get; private set; }

        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        public bool GetStartAttack()
        {
            return startAttack;
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

            fighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            DialogueControl();

            if (startAttack)
            {
                animator.SetTrigger("useCinemAnimation");

                isSecondDialogue = true;
                startAttack = false;
            }

            if (shouldJump)
            {
                StartCoroutine(ActivateCinematicAnimation());
            }

            //currentCooldownTime += Time.deltaTime;
            //currentDashCooldownTime += Time.deltaTime;

            if (enableAttackBehaviour)
            {
                EnableCamera = false;

                if (!attackBehaviourEnabled)
                {
                    navMeshAgent.enabled = true;
                    animator.SetTrigger("readyToAttack");
                    attackBehaviourEnabled = true;
                }

                if (!CanMoveTo()) return;

                fighter.Attack(playerTransform.gameObject);
                UpdateAnimator();
            }

            //AddNewAttack();
            //if (currentCooldownTime >= totalCooldownTime)
            //{
            //    UseAction(Random.Range(0, ability.Length), gameObject);
            //    currentCooldownTime = 0;
            //}
        }

        private void DialogueControl()
        {
            if (Vector3.Distance(gameObject.transform.position, playerTransform.position) <= 90 && !isSecondDialogue && !startAttack)
            {
                AIConversant conversant = GetComponent<AIConversant>();
                playerTransform.gameObject.GetComponent<PlayerConversant>().StartDialogue(conversant, conversant.GetDialogue());
                isSecondDialogue = true;
            }

            if (killedSons == 2 && isSecondDialogue)
            {
                AIConversant conversant = GetComponent<AIConversant>();
                playerTransform.gameObject.GetComponent<PlayerConversant>().StartDialogue(conversant, secondDialogue);
                killedSons = 0;
                isSecondDialogue = false;
            }
        }

        public void SomeTest()
        {
            killedSons++;
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

        private IEnumerator ActivateCinematicAnimation()
        {
            Rigidbody bossRigidbody = GetComponent<Rigidbody>();

            bossRigidbody.isKinematic = false;
            bossRigidbody.AddForce(Vector3.up * 3, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
            bossRigidbody.isKinematic = true;
            transform.position = transformCinematicTarget.transform.position;
            yield return new WaitForSeconds(0.1f);
            animator.ResetTrigger("useCinemAnimation");

            animator.SetTrigger("flip");
            bossRigidbody.isKinematic = false;
            bossRigidbody.AddForce(Vector3.down * 3, ForceMode.Impulse);

            navMeshAgent.enabled = !isFalling;

            if (!isFalling)
            {
                animator.ResetTrigger("flip");
            }

            shouldJump = false;
            enableAttackBehaviour = true;
        }

        public void Attack()
        {
            startAttack = true;
            EnableCamera = true;
        }

        void ShouldJump()
        {
            shouldJump = true;
        }

        void Land()
        {
            isFalling = !isFalling;
        }
    }
}
