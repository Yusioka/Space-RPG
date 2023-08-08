using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        CharacterController characterController;

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
            //если сработает одна из функций - другая работать не будет
            //if (health.IsDead()) return;
            //if (InteractWithCombat()) return;
            //if (InteractWithMovement()) return;

            InteractWithMovementByButtons();
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
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
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
            float speed = GetComponent<Mover>().GetLocalVelocity();
            Vector3 moveDirection = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.SimpleMove(moveDirection * 5);
        }
    }
}
