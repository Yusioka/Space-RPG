using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

public class SavingMovePosition : MonoBehaviour, ISaveable
{
    NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    [System.Serializable]
    struct MoverSaveData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState()
    {
        MoverSaveData data = new MoverSaveData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {
        MoverSaveData data = (MoverSaveData)state;
        navMeshAgent.enabled = false;
        transform.position = data.position.ToVector();
        transform.eulerAngles = data.rotation.ToVector();
        navMeshAgent.enabled = true;
    }
}
