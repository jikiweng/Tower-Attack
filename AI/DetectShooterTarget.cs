using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Attach to the enemies who attack the shooter soldier.
    public class DetectShooterTarget : DetectTarget 
    {
        protected override void getTarget(CombatTarget target)
        {
            //If the enemy is a shooter, attack it first.
            //However, if there is no shooter, the soldier will attack any soldier he detects.
            if(target.IsShooter||combatTarget==null)
                combatTarget=target;
        }
    }
}