using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Movement
{
    public interface Mover
    {
        void StartMoveAction();
        bool CanMoveTo();
        void MoveTo(Vector3 destination, float speed);

        //[SerializeField] MoverController moverController;
        //[SerializeField] float maxSpeed = 5f;

        //NavMeshAgent navMeshAgent;
        //Health health;
        //Vector3 localVelocity;

        //private void Awake()
        //{
        //    navMeshAgent = GetComponent<NavMeshAgent>();
        //    health = GetComponent<Health>();
        //}

        //private void Update()
        //{
        //    if (gameObject.tag == "Player")
        //    {
        //        GetComponent<CharacterController>().enabled = true;
        //    }
        //    UpdateButtonsAnimator();
        //    // navMesh is enabled if IsNotDead()!
        //    //  navMeshAgent.isStopped = health.IsDead();
        //}

        //private void FindTypeOfPlayersControl()
        //{
        //    if (moverController.GetIsButtonsMoving())
        //    {
        //        GetComponent<CharacterController>().enabled = true;
        //        UpdateButtonsAnimator();

        //    }
        //    else
        //    {
        //        GetComponent<CharacterController>().enabled = false;
        //        UpdateAnimator();
        //    }
        //}
        //public void StartMoveActionByMouse(Vector3 destination, float speed)
        //{
        //    GetComponent<ActionSceduler>().StartAction(this);
        //    MoveTo(destination, speed);
        //}
        //public bool CanMoveTo(Vector3 destination)
        //{
        //    NavMeshPath path = new NavMeshPath();
        //    bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        //    if (!hasPath) return false;
        //    if (path.status != NavMeshPathStatus.PathComplete) return false;
        //  //  if (GetPathLength(path) > maxNavPathLength) return false;

        //    return true;
        //}
        //public void MoveTo(Vector3 destination, float speed)
        //{
        //    //destination - место назначени€
        //    navMeshAgent.destination = destination;
        //    navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speed);
        //    navMeshAgent.isStopped = false;
        //}
        //public void StartMoveActionByButtons()
        //{
        //    GetComponent<ActionSceduler>().StartAction(this);

        //    float speed = GetSpeed();
        //    CharacterController controller = GetComponent<PlayerController>().GetCharacterController();
        //    Vector3 moveDirectionCameraSpace = FindMoveDirectionCameraSpace(FindMoveDirestion());

        //    controller.SimpleMove(moveDirectionCameraSpace * speed);
        //    RotationCameraSpace();
        //}

        //private Vector3 MotionVectorToTheCameraDirection()
        //{
        //    Vector3 cameraForward = Camera.main.transform.forward;
        //    cameraForward.y = 0; // ќбнул€ем компоненту Y, чтобы двигатьс€ по горизонтали
        //    return cameraForward;
        //}
        //private Vector3 FindMoveDirestion()
        //{ 
        //    float horizontalInput = Input.GetAxis("Horizontal");
        //    float verticalInput = Input.GetAxis("Vertical");
        //    Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        //    return moveDirection;
        //}
        //private Vector3 FindMoveDirectionCameraSpace(Vector3 moveDirection)
        //{
        //    // ѕреобразуем вектор движени€ относительно направлени€ камеры
        //    Vector3 cameraForward = MotionVectorToTheCameraDirection();
        //    Quaternion cameraRotation = Quaternion.LookRotation(cameraForward);
        //    Vector3 moveDirectionCameraSpace = cameraRotation * moveDirection;
        //    return moveDirectionCameraSpace;
        //}
        //private void RotationCameraSpace()
        //{
        //    Vector3 cameraForward = MotionVectorToTheCameraDirection();
        //    transform.rotation = Quaternion.LookRotation(cameraForward);
        //}
        //private void UpdateAnimator()
        //{
        //    Vector3 velocity = navMeshAgent.velocity;
        //    Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        //    float speed = localVelocity.z;
        //    GetComponent<Animator>().SetFloat("speedY", speed);
        //}
        //private void UpdateButtonsAnimator()
        //{
        //    float xAxis = Input.GetAxis("Horizontal");
        //    float yAxis = Input.GetAxis("Vertical");
        //    if (xAxis > 0) GetComponent<Animator>().SetFloat("speedX", 1);
        //    if (xAxis < 0) GetComponent<Animator>().SetFloat("speedX", -1);

        //    if (yAxis > 0) GetComponent<Animator>().SetFloat("speedY", 1);
        //    if (yAxis < 0) GetComponent<Animator>().SetFloat("speedY", -1);

        //    if (xAxis == 0 && yAxis == 0)
        //    {
        //        GetComponent<Animator>().SetFloat("speedX", 0);
        //        GetComponent<Animator>().SetFloat("speedY", 0);
        //    }
        //}

        //private float GetSpeed()
        //{
        //    float speed = navMeshAgent.speed;
        //    return speed;
        //}

        //public object CaptureState()
        //{
        //    return new SerializableVector3(transform.position);
        //}

        //public void RestoreState(object state)
        //{
        //    // Cast from
        //    SerializableVector3 position = (SerializableVector3)state;
        //    navMeshAgent.enabled = false;
        //    transform.position = position.ToVector();
        //    navMeshAgent.enabled = true;
        //}
    }
}
