using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Attach to the enemies who attack the fewest HP soldier.
    public class DetectFewHPTarget : DetectTarget 
    { 
        //The value is set to compare the minimum.
        private float minHP=Mathf.Infinity;

        //Find the target with minimum HP.
        protected override void getTarget(CombatTarget target)
        {
            if(target.HealthPoint<minHP)
            {
                minHP=target.HealthPoint;
                combatTarget=target;
            }
        }
    }
}