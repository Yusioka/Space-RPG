using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using RPG.Inventories;
using RPG.Core;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour, Mover, IAction
    {
        //[SerializeField] MoverController moverController;
        //[SerializeField] GameObject buttonsMovingCamera;
        //[SerializeField] GameObject mouseMovingCamera;
        //[SerializeField] float raycastRadius = 3f;
        //[SerializeField] int numberOfAbilities = 6;

        //Health health;
        //ActionStore actionStore;
        //CharacterController characterController;
        //bool isDraggingUI = false;

        [SerializeField] float maxSpeed = 5f;

        RaycastHit hit;
        bool hasHit;
        NavMeshAgent navMeshAgent;
        MoverController moverController;

        public float GetSpeed()
        {
            float speed = navMeshAgent.speed;
            return speed;
        }

        private void Awake()
        {
            moverController = GetComponent<MoverController>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (CanMoveTo())
            {
                StartMoveAction();
            }
            else
            {
                Cancel();
            }

            UpdateAnimator();
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
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    StartMoveActionByMouse(hit.point, 1f);
                }
            }
        }

        private void StartMoveActionByButtons()
        {
            GetComponent<ActionSceduler>().StartAction(this);

            float speed = GetSpeed();

            float verticalMove = Input.GetAxis("Vertical");
            float slew = Input.GetAxis("Horizontal");

            float rotation = slew * (speed / 2);
            Vector3 moveDirection = new Vector3(0, 0, verticalMove).normalized;

            transform.Rotate(0, rotation, 0);
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }

        private void StartMoveActionByMouse(Vector3 destination, float speed)
        {
            GetComponent<ActionSceduler>().StartAction(this);
            MoveTo(destination, speed);
        }
        public void MoveTo(Vector3 destination, float speed)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speed);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.Stop();
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("speedY", speed);
        }

        //public CharacterController GetCharacterController()
        //{
        //    return characterController;
        //}


        //private void Awake()
        //{
        //    health = GetComponent<Health>();
        //    actionStore = GetComponent<ActionStore>();
        //}

        //private void Start()
        //{
        //    characterController = GetComponent<CharacterController>();
        //}

        //private void Update()
        //{
        //    //  if (InteractWithUI()) return;
        //    //if (Input.GetKeyDown(KeyCode.Space))
        //    //{
        //    //    GetComponent<ItemDropper>().DropItem(InventoryItem.GetFromID("82be6903-b622-4d76-b85c-34921bb20a80"));
        //    //}

        //    SwitchCameras(mouseMovingCamera, buttonsMovingCamera);
        //    InteractWithMovementByButtons();
        //    UseAbilities();
        //    // если сработает одна из функций - другая работать не будет
        //    //   if (health.IsDead()) return;
        //    //   if (InteractWithComponent()) return;
        //    //  if (InteractWithCombat()) return;
        //}

        //private void UseAbilities()
        //{
        //    for (int i = 0; i < numberOfAbilities; i++)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Alpha1 + i))
        //        {
        //            actionStore.Use(i, this.gameObject);
        //        }
        //    }
        //}

        //private bool InteractWithComponent()
        //{
        //    foreach (RaycastHit hit in RaycastAllSorted())
        //    {
        //        IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
        //        foreach (IRaycastable raycastable in raycastables)
        //        {
        //            if (raycastable.HandleRaycast(this))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //private RaycastHit[] RaycastAllSorted()
        //{
        //    RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
        //    float[] distances = new float[hits.Length];
        //    for (int i = 0; i < hits.Length; i++)
        //    {
        //        distances[i] = hits[i].distance;
        //    }
        //    Array.Sort(distances, hits);
        //    return hits;
        //}

        //private bool InteractWithCombat()
        //{
        //    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        //    foreach (RaycastHit hit in hits)
        //    {
        //        //если нажали на объект, который имеет скрипт CombatTarget, то таргет будет равен этому объекту
        //        CombatTarget target = hit.transform.GetComponent<CombatTarget>();
        //        if (target == null) continue;

        //        if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue; // то же самое, что и ...
        //       // if (target == null) continue;

        //        if (Input.GetMouseButton(0))
        //        {       
        //            GetComponent<Fighter>().Attack(target.gameObject);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //private bool InteractWithMovementByMouse()
        //{
        //    RaycastHit hit;
        //    bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        //    if (hasHit)
        //    {
        //        if (Input.GetMouseButton(0))
        //        {
        //            GetComponent<Mover>().StartMoveActionByMouse(hit.point, 1f);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //public static Ray GetMouseRay()
        //{
        //    return Camera.main.ScreenPointToRay(Input.mousePosition);
        //}

        //private void InteractWithMovementByButtons()
        //{
        //    GetComponent<Mover>().StartMoveActionByButtons();
        //}

        //private void SwitchCameras(GameObject cameraToSwitch, GameObject cameraToActive)
        //{
        //    cameraToSwitch.SetActive(false);
        //    cameraToActive.SetActive(true);
        //}

        //private bool InteractWithUI()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        isDraggingUI = false;
        //    }
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            isDraggingUI = true;
        //        }
        //       // SetCursor(CursorType.UI);
        //        return true;
        //    }
        //    if (isDraggingUI) return true;
        //    else return false;
        //}
    }
}
