using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TowerAttack.Combat;
using TowerAttack.Stats;

namespace TowerAttack.AI
{
    //Attack to all soldiers.
    public class Soldier : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;
        [SerializeField] Transform projectileTransform=null;
        [SerializeField] GameObject attackEffect=null;

        //The below parameters come from soldierPiece.
        private int skillType;
        private float damage;
        private float basicDamage;
        private float attackTime;
        private float attackDistance;
        private int criticalRate;
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
            skillType=soldierPiece.skillType;
            speed = soldierPiece.speed;
            overrideController = soldierPiece.overrideController;

            currentTransform = GetComponent<Transform>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            combatTargetType=GetComponent<CombatTarget>().combatTargetType;
            detectTarget=GetComponentInChildren<DetectTarget>();
            ChangeSkill();

            //Set the animator override controller.
            if (overrideController != null)
                animator.runtimeAnimatorController = overrideController;
        }

        public void ChangeSkill()
        {
            basicDamage = soldierPiece.damage;
            attackTime = soldierPiece.attackTime;
            attackDistance = soldierPiece.attackDistance;
            criticalRate=soldierPiece.criticalRate;
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
            //Debug.Log("("+targetTower.towerTransform.position.x+","+targetTower.towerTransform.position.y+")");
            combatTarget = targetTower.GetComponent<CombatTarget>();
            //Debug.Log(combatTarget.MaxHealthPoint);
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
                damage=basicDamage;
                int i=Random.Range(1,101);
                if(i<=criticalRate) damage*=1.5f;
                else if(i<=30) damage*=1.2f;
                else if(i<=50) damage+=i/24;
                else if(i<=70) damage=damage+1.0f;
                else if(i<=90) damage-=i/40;
                else damage*=0.8f;

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

        //Animation Event
        void Shoot()
        {
            if (combatTarget == null) return;
            if (soldierPiece.projectile!=null)
                soldierPiece.LaunchProjectile(gameObject, projectileTransform, combatTarget,damage);
        }

        void Recover()
        {
            GetComponent<CombatTarget>().TakeDamage(damage);
            GameObject spawn=Instantiate(attackEffect,projectileTransform);
            spawn.GetComponent<Projectile>().DestroyObject();
        }

        //The attack behaviour should be cancel before moving.
        private void cancelAttack()
        {
            animator.SetTrigger("StopAttack");
            animator.ResetTrigger("IsAttack");
        }
    }
}
