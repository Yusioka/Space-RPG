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
            //���� ��������� ���� �� ������� - ������ �������� �� �����
            //if (health.IsDead()) return;
            //if (InteractWithCombat()) return;
          //  if (InteractWithMovementByMouse()) return;

            InteractWithMovementByButtons();
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
                    GetComponent<Mover>().StartMoveActionByMouse(hit.point);
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
    }
}
