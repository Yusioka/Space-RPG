using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Tag Filter", menuName = "Abilities/Filters/New Tag Filter", order = 0)]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter = "";

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> ObjectsToFilter)
        {
            foreach (var gameObject in ObjectsToFilter)
            {
                if (gameObject.CompareTag(tagToFilter))
                {
                    yield return gameObject;
                }
            }
        }
    }
}