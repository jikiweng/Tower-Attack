using System.Collections;
using System.Collections.Generic;
using TowerAttack.AI;
using TowerAttack.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.UI
{
    //Attach to the health bar.
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] CombatTarget combatTarget = null;
        
        //used to control the portion of the health bar.
        [SerializeField] RectTransform greenBar = null;

        //When damaged, the public event onDamage will call this method.
        public void ChangeHealthBar()
        {
            float fraction =
                combatTarget.HealthPoint / combatTarget.MaxHealthPoint;
            //The anchor is set to the left side so only need to change the scale on x direction.
            greenBar.localScale = new Vector3(fraction, 1.0f, 1.0f);

            //Close the health bar if the health point equals to 0 or not damaged.
            if (fraction <= 0f||fraction==1f) gameObject.SetActive(false);
        }
    }
}
