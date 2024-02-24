using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using RPG.Inventories;
using RPG.Core;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour, IMover, IAction
    {
        [SerializeField] float maxSpeed = 5f;

        [SerializeField] int numberOfAbilities = 6;
        [SerializeField] float raycastRadius = 3f;
        [SerializeField] float movingRadius = 100f;

        [SerializeField] GameObject malePrefab;
        [SerializeField] GameObject femalePrefab;

        MoverController moverController;
        CharacterPrefabController characterPrefabController;

        RaycastHit hit;
        bool hasHit;
        NavMeshAgent navMeshAgent;
        float chaseDistance = 3;
        bool isMoving;

        Health health;

        ActionStore actionStore;
        public bool IsDraggingUI { get; private set; }

        public float GetSpeed()
        {
            if (moverController.IsButtonsMoving())
            {
                return maxSpeed;
            }
            else
            {
                float speed = navMeshAgent.speed;
                return speed;
            }
        }
        public bool IsMoving()
        {
            return isMoving;
        }

        private void Awake()
        {
            characterPrefabController = GetComponent<CharacterPrefabController>();
            moverController = GetComponent<MoverController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();

            actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            ChangeCharacterPrefab();

            if (health.IsDead())
            {
                return;
            }

            if (CanMoveTo())
            {
                StartMoveAction();
            }
            else
            {
                Cancel();
            }

            if (InteractWithCombatByMouse()) return;
            if (InteractWithUI()) return;
            InteractWithCombatByButtons();
            UseAbilities();
        }

        public bool CanMoveTo()
        {
            if (!moverController.IsButtonsMoving())
            {
                hasHit = Physics.Raycast(GetMouseRay(), out hit);

                if (hasHit)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

        public void StartMoveAction()
        {
            if (moverController.IsButtonsMoving())
            {
                StartMoveActionByButtons();
                UpdateButtonsAnimator();
            }
            else
            {
                UpdateMouseAnimator();
                if (Input.GetMouseButton(1))
                {
                    StartMoveActionByMouse(hit.point, 1f);
                }
            }
        }

        private void StartMoveActionByButtons()
        {
            Cancel();
            //      GetComponent<ActionSceduler>().StartAction(this);

            if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetKey(KeyCode.W)) return;

            float speed = GetSpeed();

            float verticalMove = Input.GetAxis("Vertical");
            float slew = Input.GetAxis("Horizontal");

            if (verticalMove != 0 || slew != 0) isMoving = true;
            else isMoving = false;

            float rotation = slew * (speed / 2);
            Vector3 moveDirection = new Vector3(0, 0, verticalMove).normalized;

            if (verticalMove > 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    moveDirection = new Vector3(1, 0, 1).normalized;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    moveDirection = new Vector3(-1, 0, 1).normalized;
                }
                transform.Rotate(0, rotation, 0);
            }
            else if (verticalMove < 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    moveDirection = new Vector3(1, 0, -1).normalized;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    moveDirection = new Vector3(-1, 0, -1).normalized;
                }
                transform.Rotate(0, -rotation, 0);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                moveDirection = new Vector3(1, 0, 0).normalized;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                moveDirection = new Vector3(-1, 0, 0).normalized;
            }
            else
            {

                transform.Rotate(0, rotation, 0);
            }

            transform.Translate(moveDirection * speed * Time.deltaTime);
        }

        private void StartMoveActionByMouse(Vector3 destination, float speed)
        {
            GetComponent<ActionSceduler>().StartAction(this);
            MoveTo(destination, speed);
        }
        public void MoveTo(Vector3 destination, float speed)
        {
            isMoving = true;

            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            isMoving = false;
            navMeshAgent.Stop();
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void UpdateMouseAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speedY", speed);
        }

        private void UpdateButtonsAnimator()
        {
            float yAxis = Input.GetAxis("Vertical");

            float speedX = 0;
            float speedY = 0;

            if (yAxis > 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    speedX = 0.5f;
                    speedY = 0.5f;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    speedX = -0.5f;
                    speedY = 0.5f;
                }
                else
                {
                    speedY = 1;
                }
            }
            else if (yAxis < 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    speedX = 0.25f;
                    speedY = -0.25f;
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    speedX = -0.25f;
                    speedY = -0.25f;
                }
                else
                {
                    speedY = -0.5f;
                }
            }
            else if (Input.GetKey(KeyCode.E))
            { 
                speedX = 1f;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                speedX = -1f;
            }
            //
            if (Input.GetMouseButton(1) && Input.GetMouseButton(0) || Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                GetComponent<Animator>().SetFloat("speedY", 1);
                GetComponent<Animator>().SetFloat("speedX", 0);
            }
            //
            else
            {
                GetComponent<Animator>().SetFloat("speedX", speedX);
                GetComponent<Animator>().SetFloat("speedY", speedY);
            }
        }

        //

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.Use(i, this.gameObject);
                }
            }
        }
        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                IsDraggingUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    IsDraggingUI = true;
                }

                GetComponent<CursorController>().SetCursor(CursorType.UI);
                return true;
            }

            return IsDraggingUI;
        }
        public bool CanInteractWithComponent(GameObject interactable)
        {
            return Vector3.Distance(gameObject.transform.position, interactable.transform.position) <= chaseDistance;
        }

        private bool InteractWithCombatByMouse()
        {
            if (moverController.IsButtonsMoving()) return false;

            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);

            bool hasHitEnemy = false;
            Health newTarget = GetComponent<Fighter>().FindNewTargetInRange();

            //if (!newTarget) return false;
            //if (newTarget && newTarget.gameObject.GetComponent<Fighter>().GetTargetHealth() == health)
            //{
            //    GetComponent<Fighter>().Attack(newTarget.gameObject);
            //}

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) return false;

                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                hasHitEnemy = true;
                break;
            }

            if (Input.GetMouseButton(1))
            {
                GetComponent<Fighter>().Cancel();
          //      newTarget = null;
            }

            return hasHitEnemy;
        }

        private void InteractWithCombatByButtons()
        {
            if (!moverController.IsButtonsMoving()) return;

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            bool hasHitEnemy = false;

            Health newTarget = GetComponent<Fighter>().FindNewTargetInRange();

            if (!newTarget) return;
            if (newTarget && newTarget.gameObject.GetComponent<Fighter>().GetTargetHealth() == health)
            {
                GetComponent<Fighter>().Attack(newTarget.gameObject);
            }

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(1))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                hasHitEnemy = true;
                break;
            }

            if (!hasHitEnemy && Input.GetMouseButton(0))
            {
                GetComponent<Fighter>().Cancel();
            }
        }

        private void ChangeCharacterPrefab()
        {
            if (characterPrefabController.ChosenMale() && malePrefab.activeSelf) return;
            if (!characterPrefabController.ChosenMale() && femalePrefab.activeSelf) return;

            if (characterPrefabController.ChosenMale())
            {
                femalePrefab.SetActive(false);
                malePrefab.SetActive(true);
            }

            else
            {
                femalePrefab.SetActive(true);
                malePrefab.SetActive(false);
            }
        }
    }
}
