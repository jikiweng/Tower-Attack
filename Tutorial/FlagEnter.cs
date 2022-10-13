using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.Tutorial
{
    //Attach to detect area. Check the flag number 5.
    public class FlagEnter : MonoBehaviour 
    { 
        private PagesControl pagesControl;

        private void Start() 
        {
            pagesControl=GameObject.FindObjectOfType<PagesControl>();
        }

        //The flag will be completed only if the invader is a friend soldier.
        private void OnTriggerEnter(Collider other)
        {
            CombatTarget combatTarget = other.GetComponent<CombatTarget>();
            if (combatTarget==null||
            combatTarget.combatTargetType != CombatTargetType.Friend) return;
            
            pagesControl.CheckFlag(5);
            pagesControl.FlagComplete(5);
        }
    }
}
