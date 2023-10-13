using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using RPG.Inventories;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] MoverController moverController;
        [SerializeField] GameObject buttonsMovingCamera;
        [SerializeField] GameObject mouseMovingCamera;
        [SerializeField] float raycastRadius = 3f;
        [SerializeField] int numberOfAbilities = 6;

        Health health;
        ActionStore actionStore;
        CharacterController characterController;
        bool isDraggingUI = false;


        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        private void Awake()
        {
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            //  if (InteractWithUI()) return;
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GetComponent<ItemDropper>().DropItem(InventoryItem.GetFromID("82be6903-b622-4d76-b85c-34921bb20a80"));
            //}

            SwitchCameras(mouseMovingCamera, buttonsMovingCamera);
            InteractWithMovementByButtons();
            UseAbilities();
            // ���� ��������� ���� �� ������� - ������ �������� �� �����
            //   if (health.IsDead()) return;
            //   if (InteractWithComponent()) return;
            //  if (InteractWithCombat()) return;
        }

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

        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in RaycastAllSorted())
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                //���� ������ �� ������, ������� ����� ������ CombatTarget, �� ������ ����� ����� ����� �������
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue; // �� �� �����, ��� � ...
               // if (target == null) continue;

                if (Input.GetMouseButton(0))
                {       
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }
        private bool InteractWithMovementByMouse()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveActionByMouse(hit.point, 1f);
                }
                return true;
            }
            return false;
        }
        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void InteractWithMovementByButtons()
        {
            GetComponent<Mover>().StartMoveActionByButtons();
        }

        private void SwitchCameras(GameObject cameraToSwitch, GameObject cameraToActive)
        {
            cameraToSwitch.SetActive(false);
            cameraToActive.SetActive(true);
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
               // SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingUI) return true;
            else return false;
        }
    }
}
