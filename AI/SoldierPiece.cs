using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.AI
{
    [CreateAssetMenu(fileName = "SoldierPiece", menuName = "SoldierPiece/Make New SoldierPiece", order = 0)]
    public class SoldierPiece : ScriptableObject
    {
        //the cool down time after spawn a soldier.
        public float coolDownTime = 5f;
        //the cool down time after the soldier attacks.
        public float attackTime = 3f;
        //the distance this soldier can detect target.
        public float detectDistance = 5f;
        //the distance of it's weapon can reach.
        public float attackDistance = 5f;
        //the moving speed of this soldier.
        public float speed = 3f;

        //the int between 1~2 decides which attack type of this soldier.
        public int skillType=1;
        //the damage amount of this soldier.
        public int damage = 5;
        //the cost of spawning one of this soldier.
        public int cost=50;
        //the string that 
        public string soldierType="Soldier 1";

        public GameObject soldierPrefab;
        public AnimatorOverrideController overrideController = null;
        public CombatTargetType combatTargetType = CombatTargetType.Friend;

        private void Awake()
        {
            skillType=1;
        }

        public void SpawnSoldier(Transform position)
        {
            if (soldierPrefab == null) return;

            GameObject spawn = Instantiate(soldierPrefab, position);
            if(soldierType!="") spawn.tag = soldierType;

            if(skillType==2)
                spawn.GetComponentInChildren<Animator>().SetBool("AttackType",false);
        }

        public bool ChangeSkill(int skill)
        {
            if(skill==skillType) return false;

            GameObject[] soldiers = GameObject.FindGameObjectsWithTag(soldierType);
            foreach (GameObject soldier in soldiers)
            {
                Animator animator = soldier.GetComponentInChildren<Animator>();

                if (skill == 1)
                    animator.SetBool("AttackType", true);
                if (skill == 2)
                    animator.SetBool("AttackType", false);

            }

            skillType=skill;
            return true;
        }
    }
}
