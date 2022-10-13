using System.Collections;
using System.Collections.Generic;
using TowerAttack.AI;
using TowerAttack.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.Core
{
    //Attach to the camp prefab.
    public class Camp : MonoBehaviour
    {
        //Used to change color.
        protected Material material;
        protected MouseControl mouseControl;

        protected virtual void Start()
        {
            material = GetComponentInChildren<Renderer>().material;
            mouseControl=GameObject.FindObjectOfType<MouseControl>();
        }

        //Once clicked, change the clicked camp's color and show all the cost buttons.
        public virtual void ClickCamp()
        {
            setSelectedCamp();           
        }

        protected void setSelectedCamp()
        {
            //Find the campManager and set the this camo as selected camp.
            CampManager campManager = FindObjectOfType<CampManager>();
            campManager.SetCamp(this);

            //When any of the camp clicked, set all camps' color to default.
            SetCampColorToDefault();

            //And then, change the clicked camp's color into red.
            material.SetColor("_Color", Color.grey);      

            //Show all the cost buttons.
            if (GameObject.FindWithTag("Skill")==null) return;
            campManager.ShowTarget("Cost");
        }

        //Find all camps, and then change their colors into default color.
        public void SetCampColorToDefault()
        {
            Camp[] camps = GameObject.FindObjectsOfType<Camp>();
            foreach (Camp camp in camps)
            {
                camp.ResetCampColor();
            }
        }

        //Change the color of this camp into default color.
        public void ResetCampColor()
        {
            material.SetColor("_Color", Color.white);
        }
    }
}
