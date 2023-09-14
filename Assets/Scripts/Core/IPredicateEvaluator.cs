using UnityEngine;

namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(string predicate, string[] parameters);
    }
}
