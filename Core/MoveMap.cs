using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.Core
{
    public class MoveMap : MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GameObject.FindObjectOfType<Camp>().SetCampColorToDefault();
            
            if (!GameObject.FindWithTag("Cost").activeInHierarchy) return;
            CampManager campManager = FindObjectOfType<CampManager>();
            campManager.ShowTarget("Skill");
        }
    }
}
