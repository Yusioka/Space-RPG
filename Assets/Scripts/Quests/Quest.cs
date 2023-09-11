using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Create Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string name;
        [SerializeField] string[] objectives;

        public string GetTitle()
        {
            return name;
        }
        public int GetObjectiveCount()
        {
            return objectives.Length;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }
    }
}
