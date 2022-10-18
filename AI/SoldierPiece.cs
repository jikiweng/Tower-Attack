using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerAttack.Combat;
using TowerAttack.Title;

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

        //The parameters below will be changed with different skill.
        //the time between 2 attacks.
        public float attackTime=5f;
        //the distance the attack can reach.
        public float attackDistance=3f;
        //basic damage.
        public float damage=50f;
        //10 means 10%.
        public int criticalRate=10;
        public Projectile projectile=null;
        public AudioClip attackSound=null;

        //the int between 1~2 decides which attack type of this soldier.
        //the enemy soldier has only 1 skill.
        public int skillType=1;
        //the cost of spawning one of this soldier.
        public int cost=50;
        //the string that tells which soldier it is.
        public string soldierType="Soldier 1";

        //use when instantiate a soldier.
        public GameObject soldierPrefab;
        //every soldier shares the same animator but the animations are different. 
        public AnimatorOverrideController overrideController = null;
        //this tells which type this soldier should attack.
        public CombatTargetType combatTargetType = CombatTargetType.Friend;

        //Spawn a soldier and check the skill.
        public void SpawnSoldier(Vector3 position)
        {
            if (soldierPrefab == null) return;

            //the new soldier will be put under gameObject "Soldiers".
            Transform parent=GameObject.FindGameObjectWithTag("SoldierParent").GetComponent<Transform>();

            //Spawn soldiers and set the tag for them.
            GameObject spawn = Instantiate(soldierPrefab, position,Quaternion.identity,parent);
            if(soldierType!="") spawn.tag = soldierType;

            Fader fader=GameObject.FindObjectOfType<Fader>();
            spawn.GetComponentInChildren<AudioSource>().volume=fader.SEvolume;

            //if skillType equals to 2, change the attackType after spawn it.
            if(skillType==2)
                spawn.GetComponent<Animator>().SetBool("AttackType", false);
        }

        //Change skill for every spawned soldier. 
        //This method will return a bool. The bool shows if the current skill is the same to the clicked skill.
        public bool ChangeSkill(int skill,float attackTime,float attackDistance,float damage,int criticalRate,AudioClip attackSound)
        {
            //If the skill clicked equals to current skill, do nothing.
            if(skill==skillType) return false;
            //Change current skill type.
            BeginningSet(skill,attackTime,attackDistance,damage,criticalRate,attackSound);

            //Find all the soldiers with the same tags, and change the attackType.
            GameObject[] soldiers = GameObject.FindGameObjectsWithTag(soldierType);
            foreach (GameObject soldier in soldiers)
            {
                Animator animator = soldier.GetComponent<Animator>();

                if (skill == 1)
                    animator.SetBool("AttackType", true);
                else if (skill == 2)
                    animator.SetBool("AttackType", false);
                
                soldier.GetComponent<SoldierFriend>().ChangeSkill();
            }
            
            return true;
        }

        //These parameters are read from Skill script.
        //Called when the game starts, and everytime the skill is changed.
        public void BeginningSet(int skill,float attackTime,float attackDistance,float damage,int criticalRate,AudioClip attackSound)
        {
            this.skillType=skill;
            this.attackTime=attackTime;
            this.attackDistance=attackDistance;
            this.damage=damage;
            this.criticalRate=criticalRate;
            this.attackSound=attackSound;
        }

        //The projectile is kept in this script so the shooter will get the projectil first in this script,
        //and then move to Projectile script.
        public void LaunchProjectile(GameObject instigator, Transform projectileTransform, CombatTarget combatTarget,float damage,Color color)
        {
            Projectile projectileInstance = Instantiate(projectile, projectileTransform.position,Quaternion.identity);
            projectileInstance.AimAt(instigator, combatTarget, combatTargetType, damage,color);
        }
    }
}
