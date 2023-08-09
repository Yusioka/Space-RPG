using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Control;
using UnityEngine.EventSystems;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;

        Health health;
        Vector3 localVelocity;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            // navMesh is enabled if IsNotDead()!
        //    navMeshAgent.isStopped = health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveActionByMouse(Vector3 destination)
        {
            GetComponent<ActionSceduler>().StartAction(this);
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            //destination - место назначени€
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }
        public void StartMoveActionByButtons()
        {
            GetComponent<ActionSceduler>().StartAction(this);

            float speed = GetSpeed();
            CharacterController controller = GetComponent<PlayerController>().GetCharacterController();
            Vector3 moveDirectionCameraSpace = FindMoveDirectionCameraSpace(FindMoveDirestion());

            controller.SimpleMove(moveDirectionCameraSpace * speed);
        }
        private Vector3 FindMoveDirestion()
        { 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            return moveDirection;
        }
        private Vector3 FindMoveDirectionCameraSpace(Vector3 moveDirection)
        {
            // ѕреобразуем вектор движени€ относительно направлени€ камеры
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0; // ќбнул€ем компоненту Y, чтобы двигатьс€ по горизонтали
            Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
            Vector3 moveDirectionCameraSpace = cameraRotation * moveDirection;
            return moveDirectionCameraSpace;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
          //  GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        private float GetSpeed()
        {
            float speed = navMeshAgent.speed;
            return speed;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // Cast from
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}
