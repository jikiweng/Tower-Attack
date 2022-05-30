using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.AI
{
    public class DetectTarget : MonoBehaviour
    {
        public CombatTarget combatTarget=null;
        //Shows which type should this soldier attack.
        [SerializeField] SoldierPiece soldierPiece=null;

        private void OnTriggerStay(Collider other)
        {
            //If the collider does not have CombatTarget script, then ignore it.
            CombatTarget target = other.GetComponent<CombatTarget>();
            if(target==null) return;
            
            CombatTargetType targetType=soldierPiece.combatTargetType;
            
            //Only when the type of combatTargetType match will the soldier attack.
            if (target.combatTargetType == targetType)
            {
                combatTarget = target;
                //Close the collider until the nailed target defeated.
                GetComponent<SphereCollider>().enabled=false;
            }
        }
    }
}
