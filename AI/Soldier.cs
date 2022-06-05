using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TowerAttack.Stats;

namespace TowerAttack.AI
{
    //Attack to all soldiers.
    public class Soldier : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;

        //The below parameters come from soldierPiece.
        private int skillType;
        private float damage;
        private float attackTime;
        private float attackDistance;
        private float speed;
        private float timeBetweenAttack = Mathf.Infinity;
        //This shows the currentAction is move or attack.
        private string currentAction;

        private Transform currentTransform;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private CombatTarget combatTarget;
        private CombatTargetType combatTargetType;
        private AnimatorOverrideController overrideController;
        private DetectTarget detectTarget;

        private void Awake()
        {
            //If the skillType is 2, get below parameters from type 2.
            skillType=soldierPiece.skillType;
            if(skillType==1)
            {
                damage = soldierPiece.damage1;
                attackTime = soldierPiece.attackTime1;
                attackDistance = soldierPiece.attackDistance1;
            }
            else
            {
                damage = soldierPiece.damage2;
                attackTime = soldierPiece.attackTime2;
                attackDistance = soldierPiece.attackDistance2;
            }
            speed = soldierPiece.speed;
            overrideController = soldierPiece.overrideController;

            currentTransform = GetComponent<Transform>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            combatTargetType=GetComponent<CombatTarget>().combatTargetType;
            detectTarget=GetComponentInChildren<DetectTarget>();

            //Set the animator override controller.
            if (overrideController != null)
                animator.runtimeAnimatorController = overrideController;
        }

        void Update()
        {
            timeBetweenAttack += Time.deltaTime;

            if(combatTarget!=null&&combatTarget.tag!="Tower"&&!combatTarget.IsDead) 
            {
                attackEnemy(combatTarget);
                return;
            }

            CombatTarget target = detect();
            if (target != null)
            {
                attackEnemy(target);
                return;
            }

            //Enemies only attack friend soldiers.
            if (combatTargetType == CombatTargetType.Enemy) return;
            attackNearestTower();
        }

        //Check if detect area catch any enemy soldier.
        private CombatTarget detect()
        {
            CombatTarget target = detectTarget.combatTarget;
            if (target != null) return target;
            else
                return null;
        }

        //Set the target enemy and attack him.
        private void attackEnemy(CombatTarget target)
        {
            Vector3 targetPosition = target.targetTransform.position;
            float distance = Vector3.Distance(currentTransform.position, targetPosition);

            combatTarget = target;
            //This will arrange move and attack behaviour.
            actionManager(targetPosition, distance);
        }

        //Find the nearest tower and attack it.
        private void attackNearestTower()
        {
            //Set default value.
            Vector3 place = currentTransform.position;
            float min = Mathf.Infinity;

            //Get the towerList for all towers.
            List<Tower> towerList = FindObjectOfType<ParameterManager>().TowerList;
            Tower targetTower = towerList[0];

            //Find the nearest tower.
            foreach (Tower tower in towerList)
            {
                float distance = Vector3.Distance(tower.towerTransform.position, place);
                if (distance < min)
                {
                    targetTower = tower;
                    min = distance;
                }
            }

            //Set the distance and combatTarget.
            min = targetTower.GetDistance(min);
            combatTarget = targetTower.GetComponent<CombatTarget>();
            //This will arrange move and attack behaviour.
            actionManager(targetTower.towerTransform.position, min);
        }

        //Control move and attack behaviour.
        private void actionManager(Vector3 targetPosition, float min)
        {
            //If the target is beyond attackDistance, cancel attack behaviour and move toward it.
            if (min > attackDistance)
            {
                if (currentAction == "attack") cancelAttack();
                move(targetPosition);
            }
            //If the target is in attackDistance, cancel move behaviour and attack it.
            else
            {
                if (currentAction == "move") cancelMove();
                attack();
            }
        }

        //Use navMeshAgent to control move behaviour.
        private void move(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = speed;
            navMeshAgent.isStopped = false;

            //get the forward speed from navMeshAgent.
            float forwardSpeed = 
            currentTransform.InverseTransformDirection(navMeshAgent.velocity).z;
            //This controls whether to walk or to run.
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
            currentTransform.LookAt(combatTarget.targetTransform);
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
