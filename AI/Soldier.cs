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
        //SoldierPiece includes all the parameter for a soldier.
        public SoldierPiece soldierPiece=null;
        //This is the transform where the instantiated projectile should starts from.
        [SerializeField] Transform projectileTransform=null;
        //Some attack may have particle system or other effect.
        [SerializeField] GameObject attackEffect=null;
        [SerializeField] protected AudioSource source;

        //The below parameters come from soldierPiece.
        //the parameter shows current skill type.
        protected int skillType;
        //not the final damage, need to be calculated with criticalRate.
        protected float basicDamage;
        //the time between two attacks.
        protected float attackTime;
        //the distance this soldier can reach with his attack.
        protected float attackDistance;
        //this parameter controls the chance to deal a critical hit.
        protected int criticalRate;
        //the maximum speed for the soldier.
        protected float speed;
        //all the soldiers share one animation pattern/animator, this parameter is use to change the animation.
        protected AnimatorOverrideController overrideController;   
        protected AudioClip attackSound;   
                
                
        //Damage is calculaterd by basicDamage and criticalRate.
        protected float damage;
        //When the passing time is longer than this parameter, trigger attack and reset the passing time..
        protected float timeBetweenAttack = Mathf.Infinity;
        //This shows the currentAction is use to stop move/attack action before trigger the other one.
        protected string currentAction;
        //use to get current location.
        protected Transform currentTransform;
        //use to control navmesh behaviour.
        protected NavMeshAgent navMeshAgent;
        //use to change current animation.
        protected Animator animator;    
        //thre are 3 types of this script, different enmey AI gets different type.     
        protected DetectTarget detectTarget;
        //gets from DetectTarget script, tells which target this soldier is attacking.
        protected CombatTarget combatTarget;
        //use to identify this soldier is enemy or friend.
        protected CombatTargetType combatTargetType;

        private void Awake()
        {
            //get parameters from soldierPiece.
            skillType=soldierPiece.skillType;
            speed = soldierPiece.speed;
            overrideController = soldierPiece.overrideController;
            basicDamage = soldierPiece.damage;
            attackTime = soldierPiece.attackTime;
            attackDistance = soldierPiece.attackDistance;
            criticalRate=soldierPiece.criticalRate;
            attackSound=soldierPiece.attackSound;
            source.clip=attackSound;

            //get parameter from this gameObject.
            currentTransform = GetComponent<Transform>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            combatTargetType=GetComponent<CombatTarget>().combatTargetType;
            //detectTarget is attached to DetectArea, in the child of soldier gameObject.
            detectTarget=GetComponentInChildren<DetectTarget>();

            //Set the animator override controller.
            if (overrideController != null)
                animator.runtimeAnimatorController = overrideController;
        }

        //Use FixedUpadte to prevent different time delay due to different FPS. 
        void FixedUpdate()
        {
            //add passing time to timeBetweenAttack.
            timeBetweenAttack += Time.fixedDeltaTime;
            controller();
        }

        //The controller of friend and enemy is totally different so leave a space to override.
        protected virtual void controller() { return;}

        //Set the target and attack him.
        protected void attackEnemy(CombatTarget target)
        {
            //get the position of target and get the distance between this soldier adn target.
            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(currentTransform.position, targetPosition);

            combatTarget = target;
            //this will arrange move and attack behaviour.
            actionManager(targetPosition, distance);
        }

        //Control move and attack behaviour.
        //Cancel move/attack behaviour if the current action is the other one.
        protected void actionManager(Vector3 targetPosition, float min)
        {
            //If the target is beyond attackDistance, move toward him.
            if (min > attackDistance)
            {
                if (currentAction == "attack") cancelAttack();
                move(targetPosition, speed);
            }
            //If the target is in attackDistance, attack him.
            else
            {
                if (currentAction == "move") cancelMove();
                attack();
            }
        }

        //Use navMeshAgent to control move behaviour.
        //There are 2 kinds of speed, moving speed and patroling speed.
        protected void move(Vector3 destination,float moveSpeed)
        {
            //set the destination and speed for the move behaviour.
            navMeshAgent.destination = destination;
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.isStopped = false;

            //this controls whether to walk or to run.
            animator.SetFloat("Speed", moveSpeed);

            //mark current action as move.
            currentAction = "move";
        }

        //The move behaviour should be cancel before attacking.
        protected void cancelMove()
        {
            animator.SetFloat("Speed", 0f);
            navMeshAgent.isStopped = true;
        }

        //Handle the attack behaviour, including the damage calculation.
        protected void attack()
        {
            //face to the target
            currentTransform.LookAt(combatTarget.transform);
            //Only when the time since last attack is longer than attackTime will the soldier attack.
            if (timeBetweenAttack >= attackTime)
            {
                damage=basicDamage;
                int i=Random.Range(1,101);
                //critical calulation comes first. if the rate is 40, then means 40% to deal a critical hit.
                if(i<=criticalRate) damage*=1.5f;
                //the below calculation will be overrided by critical hit if the rate is higher.
                else if(i<=30) damage*=1.2f;
                else if(i<=50) damage+=i/24;
                else if(i<=70) damage=damage+1.0f;
                else if(i<=90) damage-=i/40;
                else damage*=0.8f;

                //reset the trigger before triggered to prevent bug.
                animator.ResetTrigger("StopAttack");
                animator.SetTrigger("IsAttack");
                //reset timeBetweenAttack.
                timeBetweenAttack = 0f;
            }
            //mark current action as attack.
            currentAction = "attack";
        }

        //The "Hit" event will be triggered in attack animation.
        //There must be a "Hit" event set on the moment the target take damage.
        void Hit()
        {
            //if the target is killed by other soldier, deal no damage.
            if (combatTarget == null) return;
            //else, deal a damage on target.
            combatTarget.TakeDamage(damage);

            source.Play();
        }

        //The "Shoot" event will be triggered in attack animation.
        void Shoot()
        {
            if (combatTarget == null) return;
            //every shooter soldierPiece has a projectile parameter.
            //instantiate the projectile to attack the enemy.
            if (soldierPiece.projectile!=null)
                soldierPiece.LaunchProjectile(gameObject, projectileTransform, combatTarget,damage);
                
            source.Play();
        }

        //The "Recover" event will be triggered in attack animation.
        void Recover()
        {
            StartCoroutine(recoverBehaviour());            
            source.Play();
        }

        //the particle system should be exist for a while after recover, so use coroutine function. 
        private IEnumerator recoverBehaviour() 
        {
            //the damage is set to be negative so that HP can increase with TakeDamage method.
            GetComponent<CombatTarget>().TakeDamage(damage);
            //the partice effect is not a projectile but can still use the position.
            GameObject spawn=Instantiate(attackEffect,projectileTransform);
            //keep the particle effect for w while.
            yield return new WaitForSeconds(2);
            spawn.GetComponent<Projectile>().DestroyObject();
            yield return null;
        }

        //The attack behaviour should be cancel before moving.
        protected void cancelAttack()
        {
            animator.SetTrigger("StopAttack");
            animator.ResetTrigger("IsAttack");
        }
    }
}
