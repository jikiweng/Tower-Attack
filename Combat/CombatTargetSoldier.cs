using UnityEngine;

namespace TowerAttack.Combat
{
    public class CombatTargetSoldier : CombatTarget
    {         
        //When a soldier dies, play the die animation and destroy the gameobject after that.
        protected override void die()
        {
            GetComponent<Animator>().SetTrigger("IsDie");
            Invoke("destroyObject", 2);
        }

        //Create a new function for Invoke, so that there can be a delay after the soldier die.
        private void destroyObject()
        {
            if(gameObject.tag!="Untagged") 
            {
                Balance balance=GameObject.FindObjectOfType<Balance>();

                if(!balance.HasMoney) balance.CheckIfLose();
            }
            Destroy(gameObject);
        }
    }
}
