using RPG.Movement;
using RPG.Core;
using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using RPG.Inventories;
using GameDevTV.Utils;
using System.Collections.Generic;
using UnityEngine.AI;
using JetBrains.Annotations;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] float autoAttackRange = 4;

        Health target;
        Equipment equipment;
        float timeSinceLastAttack = 0;
        WeaponConfig currentWeaponConfig = null;
        Weapon currentWeapon;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            SetupDefaultWeapon();

            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || GetComponent<Health>().IsDead()) return;
            if (target.IsDead())
            {
                target = FindNewTargetInRange();
                if (target == null) return;
            }

            if (!GetIsInRange(target.transform))
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                GetComponent<IMover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                AttackBehaviour();
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            if (weapon != null)
            {
                return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            }
            else
            {
                return null;
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon = AttachWeapon(weapon);
        }
        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        public Health GetTargetHealth()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            if (target == null) return;
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > currentWeaponConfig.GetTimeBetweenAttacks())
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
            else
            {
                ResetTriggerAttack();
            }
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            if (!GetIsInRange(target.transform)) return;

            float damage = currentWeaponConfig.GetDamage();

            if (currentWeapon != null)
            {
                currentWeapon.OnHit();
            }
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        private Health FindNewTargetInRange()
        {
            Health bestCandidate = null;
            float bestDistance = Mathf.Infinity;
            foreach (var candidate in FindAllTargetsInRange())
            {
                if (candidate == null) continue;
                float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
                if (candidateDistance < bestDistance)
                {
                    bestCandidate = candidate;
                    bestDistance = candidateDistance;
                }
            }
            return bestCandidate;
        }
        private IEnumerable<Health> FindAllTargetsInRange()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, autoAttackRange, Vector3.up);
            foreach (var hit in raycastHits)
            {
                Health health = hit.transform.GetComponent<Health>();
                if (health == null) continue;
                if (health.IsDead()) continue;
                if (health.gameObject == gameObject) continue;
                yield return health;
            }
        }

        void Shoot()
        {
            Hit();
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand)
            {
                return rightHandTransform;
            }
            else
            {
                return leftHandTransform;
            }
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) <= currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null || combatTarget == gameObject) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void Attack(GameObject combatTarget)
        {
         //   GetComponent<ActionSceduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            ResetTriggerAttack();
            target = null;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void ResetTriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }
}
