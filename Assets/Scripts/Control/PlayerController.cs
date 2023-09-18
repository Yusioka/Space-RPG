using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] MoverController moverController;
        [SerializeField] GameObject buttonsMovingCamera;
        [SerializeField] GameObject mouseMovingCamera;
        [SerializeField] float raycastRadius = 3f;

        Health health;
        CharacterController characterController;


        public CharacterController GetCharacterController()
        {
            return characterController;
        }

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (moverController.GetIsButtonsMoving())
            {
                SwitchCameras(mouseMovingCamera, buttonsMovingCamera);
                InteractWithMovementByButtons();
            }
            else
            {
                SwitchCameras(buttonsMovingCamera, mouseMovingCamera);
                if (InteractWithMovementByMouse()) return;
            }
            // если сработает одна из функций - другая работать не будет
            if (health.IsDead()) return;
            //if (InteractWithComponent()) return;
          //  if (InteractWithCombat()) return;
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
                //если нажали на объект, который имеет скрипт CombatTarget, то таргет будет равен этому объекту
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue; // то же самое, что и ...
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
        private static Ray GetMouseRay()
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
    }
}
