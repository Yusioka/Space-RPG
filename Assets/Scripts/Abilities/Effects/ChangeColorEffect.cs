using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Change Color Effect", menuName = "Abilities/Effects/New Change Color Effect", order = 0)]
    public class ChangeColorEffect : EffectStrategy
    {
        [SerializeField] bool changeUser;
        [SerializeField] Color color;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            if (changeUser)
            {
                foreach (Transform children in data.GetUser().transform)
                {
                    Debug.Log(children.gameObject.name);
                    children.GetComponentInChildren<Renderer>().material.color = color;
                }

                if (destroyDelay > 0)
                {
                    yield return new WaitForSeconds(destroyDelay);

                    foreach (Transform children in data.GetUser().transform)
                    {
                        children.GetComponentInChildren<Renderer>().material.color = Color.white;
                    }
                }

                finished();
            }
 
            else
            {
                foreach (var target in data.GetTargets())
                {
                    foreach (Transform children in target.transform)
                    {
                        if (children.GetComponentInChildren<Renderer>() == null) continue;

                        children.GetComponentInChildren<Renderer>().material.color = color;
                    }

                    if (destroyDelay > 0)
                    {
                        yield return new WaitForSeconds(destroyDelay);

                        foreach (Transform children in target.transform)
                        {
                            if (children.GetComponentInChildren<Renderer>() == null) continue;

                            children.GetComponentInChildren<Renderer>().material.color = Color.white;
                        }
                    }

                    finished();
                }
            }
        }
    }
}