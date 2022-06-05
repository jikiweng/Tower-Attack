using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.Core
{
    //Attach to the map area.
    public class MoveMap : MonoBehaviour,IPointerClickHandler
    {
        //When the map area clicked, close the cost buttons and show the skill icons.
        public void OnPointerClick(PointerEventData eventData)
        {
            //Change the selected camp color into default.
            //Then set the selected camp to null.
            CampManager campManager = GameObject.FindObjectOfType<CampManager>();
            campManager.GetCamp().ResetCampColor();
            campManager.SetCamp();
            
            //This will trigger the method to adjust panel display.
            if (!GameObject.FindWithTag("Cost").activeInHierarchy) return;
            campManager.ShowTarget("Skill");
        }
    }
}
