using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.Combat
{
    //Attach to soldiers' detect areas.
    public class DetectTarget : MonoBehaviour
    {
        public CombatTarget combatTarget=null;
        //Shows which type should this soldier attack.
        [SerializeField] CombatTargetType targetType=CombatTargetType.Friend;

        private void OnTriggerStay(Collider other)
        {
            //If the collider does not have CombatTarget script, then ignore it.
            CombatTarget target = other.GetComponent<CombatTarget>();
            if(target==null) return;
            
            //Only when the type of combatTargetType match will the soldier attack.
            if (target.combatTargetType == targetType) combatTarget = target;
        }
    }
}
