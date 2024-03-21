using RPG.Movement;
using RPG.Core;
using UnityEngine;
using RPG.Attributes;
using RPG.Inventories;
using System.Collections.Generic;
using UnityEngine.AI;
using RPG.Control;
using RPG.Dialogue;
using UnityEngine.Events;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] float autoAttackRange = 4;

        public UnityEvent OnAttack;

        Health target;
        Equipment equipment;
        float timeSinceLastAttack = 0;
        WeaponConfig currentWeaponConfig = null;
        Weapon currentWeapon;
        bool canMoveToEnemy = false;

        private void Start()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = AttachWeapon(currentWeaponConfig);

            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
                equipment.equipmentUpdated += UpdateEquipment;
                UpdateWeapon();
                UpdateEquipment();
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;


            if (gameObject.tag == "Player" && timeSinceLastAttack >= 25 && target == null)
            {
                Health health = GetComponent<Health>();
                if (health.HealthPoints < health.GetMaxHealthPoints())
                {
                    health.Heal(GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate));

                    if (health.HealthPoints > health.GetMaxHealthPoints())
                    {
                        health.HealthPoints = health.GetMaxHealthPoints();
                    }
                }
            }

            if (target == null) return;

            if (GetComponent<Health>().IsDead() || target.IsDead())
            {
                Cancel();
            }

            //if (target.IsDead())
            //{
            //    target = FindNewTargetInRange();
            //    if (target == null) return;
            //}

            if (target && Input.GetMouseButtonDown(0)) canMoveToEnemy = true;

            if (gameObject == GameObject.FindWithTag("Player"))
            {
                if (!GetIsInRange(target.transform) && !GameObject.FindWithTag("Player").GetComponent<MoverController>().IsButtonsMoving() && canMoveToEnemy)
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
                        GetComponent<IMover>().MoveTo(target.transform.position, GetComponent<BossController>().GetSpeed());
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
            if (gameObject == GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<MoverController>().IsButtonsMoving() && Vector3.Dot((target.transform.position - GameObject.FindWithTag("Player").transform.position).normalized, GameObject.FindWithTag("Player").transform.forward) < 0.96) return;


            if (gameObject.tag != "Player" || gameObject.tag == "Player" && !GameObject.FindWithTag("Player").GetComponent<MoverController>().IsButtonsMoving())
            {
               transform.LookAt(target.transform);
            }

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

            float damage = currentWeaponConfig.GetDamage() + (currentWeaponConfig.GetDamage() + GetComponent<BaseStats>().GetStat(Stat.Damage));

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

        public Health FindNewTargetInRange()
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
            if (combatTarget.GetComponent<AIConversant>() && combatTarget.GetComponent<AIConversant>().enabled == true) return false;
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
            canMoveToEnemy = false;
            target = null;
        }

        private void TriggerAttack()
        {
            OnAttack.Invoke();

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
