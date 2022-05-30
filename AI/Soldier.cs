using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TowerAttack.Stats;

namespace TowerAttack.AI
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;

        private int damage;
        private float attackTime;
        private float attackDistance;
        private float speed;
        private float timeBetweenAttack = Mathf.Infinity;
        private string currentAction;

        private Transform currentTransform;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private CombatTarget combatTarget;
        private AnimatorOverrideController overrideController;

        private void Awake()
        {
            damage = soldierPiece.damage;
            attackTime = soldierPiece.attackTime;
            attackDistance = soldierPiece.attackDistance;
            speed = soldierPiece.speed;
            overrideController = soldierPiece.overrideController;

            currentTransform = GetComponent<Transform>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            //Set the animator override controller.
            if (overrideController != null)
                animator.runtimeAnimatorController = overrideController;
        }

        // Update is called once per frame
        void Update()
        {
            timeBetweenAttack += Time.deltaTime;

            CombatTarget target = detect();
            if (target != null && !target.IsDead)
            {
                Vector3 targetPosition = target.GetComponent<Transform>().position;
                float distance = Vector3.Distance(currentTransform.position, targetPosition);

                combatTarget = target;
                actionManager(targetPosition, distance);
                return;
            }

            //Enemies only attack friend soldiers.
            if (GetComponent<CombatTarget>().combatTargetType == CombatTargetType.Enemy) return;
            attackNearestTower();
        }

        private CombatTarget detect()
        {
            CombatTarget target = GetComponentInChildren<DetectTarget>().combatTarget;
            if (target != null) return target;
            else
                return null;
        }

        private void attackNearestTower()
        {
            Vector3 place = currentTransform.position;
            Vector3 towerPosition = place;
            float min = Mathf.Infinity;
            List<Tower> towerList = GameObject.FindObjectOfType<ParameterManager>().TowerList;

            if (towerList.Count == 0) return;
            Tower targetTower = towerList[0];

            foreach (Tower tower in towerList)
            {
                Transform towerTransform = tower.GetComponent<Transform>();
                Vector3 destination = towerTransform.position;

                float distance = Vector3.Distance(destination, place);
                if (distance < min)
                {
                    targetTower = tower;
                    towerPosition = destination;
                    min = distance;
                }
            }

            min = targetTower.GetDistance(min);
            combatTarget = targetTower.GetComponent<CombatTarget>();
            actionManager(towerPosition, min);
        }

        private void actionManager(Vector3 targetPosition, float min)
        {
            if (min > attackDistance)
            {
                if (currentAction == "attack") cancelAttack();
                move(targetPosition);
            }
            else
            {
                if (currentAction == "move") cancelMove();
                attack();
            }
        }

        private void move(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = speed;
            navMeshAgent.isStopped = false;

            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = currentTransform.InverseTransformDirection(velocity);
            float forwardSpeed = localVelocity.z;
            animator.SetFloat("Speed", forwardSpeed);

            currentAction = "move";
        }

        //The move behaviour should be cancel before attacking.
        private void cancelMove()
        {
            animator.SetFloat("Speed", 0f);
            navMeshAgent.isStopped = true;
        }

        private void attack()
        {
            //face to the target
            currentTransform.LookAt(combatTarget.GetComponent<Transform>());
            //control the cooldown time between 2 attacks 
            //Only when the time since last attack is bigger than attackTime will the soldier attack.
            if (timeBetweenAttack >= attackTime)
            {
                animator.ResetTrigger("StopAttack");
                animator.SetTrigger("IsAttack");
                timeBetweenAttack = 0f;
            }
            currentAction = "attack";
        }

        //This will trigger the "Hit" event in attack animation.
        //There must be a "Hit" event set on the moment the target take damage.
        public void Hit()
        {
            if (combatTarget == null) return;
            combatTarget.TakeDamage(damage);
        }

        //The attack behaviour should be cancel before moving.
        private void cancelAttack()
        {
            animator.SetTrigger("StopAttack");
            animator.ResetTrigger("IsAttack");
        }
    }
}
