using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerAttack.UI;
using TowerAttack.AI;

namespace TowerAttack.Core
{
    public class Camp : MonoBehaviour, IRaycastable, IPointerClickHandler
    {
        public CursorType GetCursorType()
        {
            return CursorType.Select;
        }

        public bool HandleRaycast()
        {
            return true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CampManager campManager=FindObjectOfType<CampManager>();
            campManager.SetCamp(this);

            //When any of the camp clicked, set all camps' color to default.
            SetCampColorToDefault();
            //And then, change the clicked camp's color into red.
            Material material = GetComponentInChildren<Renderer>().material;
            material.SetColor("_Color", Color.red);

            if (!GameObject.FindWithTag("Skill").activeInHierarchy) return;
            campManager.ShowTarget("Cost");
        }

        public void SetCampColorToDefault()
        {
            Camp[] camps = GameObject.FindObjectsOfType<Camp>();
            foreach (Camp camp in camps)
            {
                Material campMaterial = camp.GetComponentInChildren<Renderer>().material;
                campMaterial.SetColor("_Color", Color.white);
            }
        }
    }
}