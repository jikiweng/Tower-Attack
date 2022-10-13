using System.Collections;
using System.Collections.Generic;
using TowerAttack.AI;
using TowerAttack.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerAttack.Core
{
    //Attach to all skill icons.
    public class ChangeSkill : MonoBehaviour, IPointerClickHandler
    {
        //the cool down time after the soldier attacks.
        [SerializeField] float attackTime = 3f;
        //the distance of it's weapon can reach.
        [SerializeField] float attackDistance = 5f;
        //the damage amount of this soldier.
        [SerializeField] float damage = 5f;
        [SerializeField] Projectile projectile=null;
        [SerializeField] int criticalRate=10;
        [SerializeField] AudioClip attackSound=null;

        [SerializeField] SoldierPiece soldierPiece = null;
        //used to call the SetFalse method for hide icon.
        [SerializeField] ChangeSkill hideTarget = null;
        //goldFrame should shows off when clicked.
        [SerializeField] GameObject goldFrame = null;
        [SerializeField] Image image = null;
        [SerializeField] int skillType = 1;

        void Start()
        {
            if(skillType!=1) return;

            //the skill is set to be skill 1.
            soldierPiece.BeginningSet(skillType,attackTime,attackDistance,damage,criticalRate,attackSound);
            if(projectile!=null)
                soldierPiece.projectile=projectile;
        }

        //When the icon clicked, open up this icon and close the other one.
        public void OnPointerClick(PointerEventData eventData)
        {
            //if the clicked skill is the same as current skill, do nothing.
            if (!changeParameters())
                return;

            setTrue();
            hideTarget.setFalse();
        }

        //Some of the parameters in SoldierPiece script are read from this script.
        //So everytime the skill changed, those parameters should be updated.
        private bool changeParameters()
        {
            //the bool will be false if the clicked skill is the same as current skill.
            bool result=soldierPiece.ChangeSkill(skillType,attackTime,attackDistance,damage,criticalRate,attackSound);
            //not every soldier has projectile. do nothing if the soldier is not a shooter.
            if(projectile!=null)
                soldierPiece.projectile=projectile;
            return result;
        }

        //Show the goldFrame and change the color to dark.
        private void setTrue()
        {
            goldFrame.SetActive(true);
            image.color = new Color32(255, 255, 255, 255);
        }
        //Close the goldFrame and change the color to bright.
        public void setFalse()
        {
            goldFrame.SetActive(false);
            image.color = new Color32(100, 100, 100, 255);
        }
    }
}
