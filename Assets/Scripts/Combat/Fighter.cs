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
using RPG.Control;

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
                equipment.equipmentUpdated += UpdateEquipment;
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

            if (gameObject == GameObject.FindWithTag("Player"))
            {
                if (!GetIsInRange(target.transform) && !GameObject.FindWithTag("Player").GetComponent<MoverController>().IsButtonsMoving())
                {
                    GetComponent<NavMeshAgent>().enabled = true;
                    GetComponent<IMover>().MoveTo(target.transform.position, 1f);
                }

                else if (GetIsInRange(target.transform))
                {
                    GetComponent<NavMeshAgent>().enabled = false;
                    AttackBehaviour();
                }
            }
            
            else
            {
                if (!GetIsInRange(target.transform))
                {
                    GetComponent<NavMeshAgent>().enabled = true;

                    if (GetComponent<EnemyController>() != null)
                    {
                        GetComponent<IMover>().MoveTo(target.transform.position, GetComponent<EnemyController>().GetSpeed());
                    }

                    else
                    {
                        GetComponent<IMover>().MoveTo(target.transform.position, 1f);
                    }
                }

                else if (GetIsInRange(target.transform))
                {
                    GetComponent<NavMeshAgent>().enabled = false;
                    AttackBehaviour();
                }
            }

            GetComponent<NavMeshAgent>().enabled = true;
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

        private void EquipItem(EquipableItem equipableItem)
        {
            if (equipableItem != null)
            {
                equipableItem.Equip(gameObject.transform);
            }
        }

        private void UpdateEquipment()
        {
            var body = equipment.GetItemInSlot(EquipLocation.Body) as EquipableItem;
            var helmet = equipment.GetItemInSlot(EquipLocation.Helmet) as EquipableItem;
            if (body != null)
            {
                EquipItem(body);
            }
            if (helmet != null)
            {
                EquipItem(helmet);
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

            int randomIndex = Random.Range(0, 2);

            if (HasTrigger(GetComponent<Animator>(), "secondAttack"))
            {
                if (randomIndex == 0) GetComponent<Animator>().SetTrigger("attack");
                else if (randomIndex == 1) GetComponent<Animator>().SetTrigger("secondAttack");
            }

            else
            {
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        private void ResetTriggerAttack()
        {
            if (HasTrigger(GetComponent<Animator>(), "secondAttack"))
            {
                GetComponent<Animator>().ResetTrigger("secondAttack");
            }

            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        private bool HasTrigger(Animator animator, string triggerName)
        {
            AnimatorControllerParameter[] parameters = animator.parameters;

            foreach (var parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name == triggerName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
