using System.Collections;
using System.Collections.Generic;
using TowerAttack.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerAttack.Core
{
    //Attach to all skill icons.
    public class ChangeSkill : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] SoldierPiece soldierPiece = null;
        //used to call the SetFalse method for hide icon.
        [SerializeField] ChangeSkill hideTarget = null;
        //greyFrame should shows off when clicked.
        [SerializeField] GameObject greyFrame = null;
        [SerializeField] Image image = null;
        [SerializeField] int skillType = 1;

        //When the icon clicked, open up this icon and close the other one.
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!soldierPiece.ChangeSkill(skillType)) return;

            setTrue();
            hideTarget.setFalse();
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
