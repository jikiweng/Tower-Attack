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

        [SerializeField] SoldierPiece soldierPiece = null;
        //used to call the SetFalse method for hide icon.
        [SerializeField] ChangeSkill hideTarget = null;
        //greyFrame should shows off when clicked.
        [SerializeField] GameObject greyFrame = null;
        [SerializeField] Image image = null;
        [SerializeField] int skillType = 1;

        void Start()
        {
            if(skillType==1)
                soldierPiece.BeginningSet(skillType,attackTime,attackDistance,damage,criticalRate);
            if(projectile!=null)
                soldierPiece.projectile=projectile;
        }

        //When the icon clicked, open up this icon and close the other one.
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!changeParameters())
                return;

            setTrue();
            hideTarget.setFalse();
        }

        private bool changeParameters()
        {
            bool result=soldierPiece.ChangeSkill(skillType,attackTime,attackDistance,damage,criticalRate);
            if(projectile!=null)
                soldierPiece.projectile=projectile;
            return result;
        }

        //Show the greyFrame and change the color to dark.
        private void setTrue()
        {
            greyFrame.SetActive(true);
            image.color = new Color32(50, 50, 50, 255);
        }
        //Close the greyFrame and change the color to bright.
        public void setFalse()
        {
            greyFrame.SetActive(false);
            image.color = new Color32(255, 255, 255, 255);
        }
    }
}
