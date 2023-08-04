using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectle : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] bool isHoming = true;
        [SerializeField] float maxLifeTime = 3f;

        Health target = null;
        GameObject instigator = null;
        float damage = 0;


        private void Start()
        {
            transform.LookAt(GetAimLocation());

        }
        private void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            // Destroy(what, when);
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }    
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            target.TakeDamage(instigator, damage);
            Destroy(gameObject);
        }
    }
}
