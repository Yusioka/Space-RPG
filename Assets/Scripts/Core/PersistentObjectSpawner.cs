using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        // static - another class can't include it
        static bool hasSpawned = false;

        // The singleton
        // Instantiate the object which won't destroy even if the scenes changes!
        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
