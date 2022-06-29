using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Controls all the parameters for the soldier.
    [CreateAssetMenu(fileName = "SoldierPiece", menuName = "SoldierPiece/Make New SoldierPiece", order = 0)]
    public class SoldierPiece : ScriptableObject
    {
        //the cool down time after spawn a soldier.
        public float coolDownTime = 5f;
        //the moving speed of this soldier.
        public float speed = 3f;
        //the distance this soldier can detect target.
        public float detectDistance = 5f;

        public float attackTime=5f;
        public float attackDistance=3f;
        public float damage=50f;
        public int criticalRate=10;
        public Projectile projectile=null;

        //the int between 1~2 decides which attack type of this soldier.
        public int skillType=1;
        //the cost of spawning one of this soldier.
        public int cost=50;
        //the string that tells which soldier it is.
        public string soldierType="Soldier 1";

        public GameObject soldierPrefab;
        public AnimatorOverrideController overrideController = null;
        //this tells which type this soldier should attack.
        public CombatTargetType combatTargetType = CombatTargetType.Friend;

        public void SpawnSoldier(Transform position)
        {
            if (soldierPrefab == null) return;

            //Spawn soldiers and set the tag for them.
            GameObject spawn = Instantiate(soldierPrefab, position);
            if(soldierType!="") spawn.tag = soldierType;

            //if skillType equals to 2, change the attackType after spawn it.
            if(skillType==2)
                spawn.GetComponent<Animator>().SetBool("AttackType", false);
        }

        //Change skill for every spawned soldier. 
        public bool ChangeSkill(int skill,float attackTime,float attackDistance,float damage,int criticalRate)
        {
            //If the skill clicked equals to current skill, do nothing.
            if(skill==skillType) return false;

            //Find all the soldiers with the same tags, and change the attackType.
            GameObject[] soldiers = GameObject.FindGameObjectsWithTag(soldierType);
            foreach (GameObject soldier in soldiers)
            {
                Animator animator = soldier.GetComponent<Animator>();

                if (skill == 1)
                    animator.SetBool("AttackType", true);
                else if (skill == 2)
                    animator.SetBool("AttackType", false);
                
                BeginningSet(skill,attackTime,attackDistance,damage,criticalRate);
                soldier.GetComponent<Soldier>().ChangeSkill();
            }

            //Change current skill type.
            skillType=skill;
            return true;
        }

        public void BeginningSet(int skill,float attackTime,float attackDistance,float damage,int criticalRate)
        {
            this.attackTime=attackTime;
            this.attackDistance=attackDistance;
            this.damage=damage;
            this.criticalRate=criticalRate;
        }

        public void LaunchProjectile(GameObject instigator, Transform projectileTransform, CombatTarget combatTarget,float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, projectileTransform.position,Quaternion.identity);
            projectileInstance.AimAt(instigator, combatTarget, combatTargetType, damage);
        }
    }
}
