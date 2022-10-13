using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.AI
{
    //Attach to soldiers' detect areas.
    public class DetectTarget : MonoBehaviour
    {
        //This is used to know which target this soldier is attacking.
        //Do not set this parameter in Unity.
        public CombatTarget combatTarget=null;
        //Shows which type should this soldier attack.
        //Should be Friend for enemy soldiers; be Enemy for friend soldier.
        [SerializeField] CombatTargetType targetType=CombatTargetType.Friend;

        //After defeat a soldier, the next target might be in the detect area already 
        //so use OnTriggerStay instead of OnTriggerEnter.
        private void OnTriggerStay(Collider other)
        {
            //If the collider does not have CombatTarget script, then ignore it.
            CombatTarget target = other.GetComponent<CombatTarget>();
            if(target==null) return;
            
            //Only when the type of combatTargetType match will the soldier attack.
            if (target.combatTargetType == targetType) getTarget(target);
        }

        //This method will be overrided by different enemy type.
        //Different enemy type get different target.
        protected virtual void getTarget(CombatTarget target)
        {
            combatTarget=target;
        }
    }
}
