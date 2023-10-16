using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] bool isHoming = true;
        [SerializeField] float maxLifeTime = 3f;
        [SerializeField] GameObject[] destroyOnHit = null;

        Health target = null;
        Vector3 targetPoint = default;
        GameObject instigator = null;
        float damage = 0;


        private void Start()
        {
            transform.LookAt(GetAimLocation());

        }
        private void Update()
        {
            if (target == null)
            {

            }

            if (target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(GameObject instigator, Health target, float damage)
        {
            SetTarget(instigator, target, damage);
        }
        public void SetTarget(GameObject instigator, Vector3 targetPoint, float damage)
        {
            SetTarget(instigator, targetPoint, damage);
        }
        public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            // Destroy(what, when);
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }

            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }    
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (target != null && health != target) return;
            if (health == null || health.IsDead()) return;
            if (other.gameObject == instigator) return;

            health.TakeDamage(instigator, damage);


            speed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
        }
    }
}
