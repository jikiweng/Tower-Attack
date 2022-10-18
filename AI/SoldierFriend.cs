using UnityEngine;
using System.Collections.Generic;
using TowerAttack.Stats;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Attach to all freind soldier.
    public class SoldierFriend : Soldier 
    { 
        private List<Tower> towerList=new List<Tower>();
        //use to check if the player wins or not.
        private bool isWin=false;

        void Start() 
        {
            //get the towerlist fom ParameterManager.
            towerList = FindObjectOfType<ParameterManager>().TowerList;
        }

        //Override the controller in Soldier script. Called in FixedUpdate().
        protected override void controller()
        {
            //if the player wins, do nothing.
            if(isWin) return;
            
            //friend soldier has 2 type of target, tower and enemy soldiers.
            //friend soldier always attack enemy soldier before tower.
            if(combatTarget!=null&&combatTarget.tag!="Tower"&&!combatTarget.IsDead) 
            {
                attackEnemy(combatTarget);
                return;
            }

            //if the target is tower or the previous target is dead, find a new one.
            CombatTarget target = detectTarget.combatTarget;
            if(target!=null) 
            {
                combatTarget=target;
                attackEnemy(target);
                return;
            }

            //if the tower is in the soldier's attack range, cancle moving and attack it.
            if(combatTarget!=null&&towerList.Contains(combatTarget.GetComponent<Tower>()))
            {
                if(Vector3.Distance(currentTransform.position,combatTarget.transform.position)<=attackDistance)            
                {       
                    cancelMove();                
                    attack();
                }
                //else return;
            }
            
            //if none of the above, move toward the nearest tower.
            moveToNearestTower();
        }

        //Friend soldier has 2 skills. The following paramter should be updated after changing the sklil.
        public void ChangeSkill()
        {
            basicDamage = soldierPiece.damage;
            attackTime = soldierPiece.attackTime;
            attackDistance = soldierPiece.attackDistance;
            criticalRate=soldierPiece.criticalRate;
            attackSound=soldierPiece.attackSound;
            source.clip=attackSound;
        }

        //Find the nearest tower and move toward it.
        private void moveToNearestTower()
        {
            //Set default value.
            Vector3 place = currentTransform.position;
            float min = Mathf.Infinity;

            //Get the towerList for all towers.
            Tower targetTower = towerList[0];

            //Find the nearest tower.
            foreach (Tower tower in towerList)
            {
                float distance = Vector3.Distance(tower.towerPosition, place);
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
            actionManager(targetTower.towerPosition, min);
        }

        //If the player wins, the soldier needs to stop and play win animation.
        public void Win()
        {
            cancelAttack();
            cancelMove();
            isWin=true;
            //trigger win animation.
            animator.SetTrigger("IsWin");
            //destroy this script.
            this.enabled=false;
        }
    }
}
