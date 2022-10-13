using UnityEngine;
using TowerAttack.Tutorial;
using TowerAttack.Core;

namespace TowerAttack.UI
{
    //Attach to the main camera in tutorial.
    public class TutorialMouseControl : MouseControl 
    { 
        //check point Number 7.
        private int flag=7;
        private PagesControl pagesControl;

        private void Start() 
        {
            pagesControl=GameObject.FindObjectOfType<PagesControl>();
        }

        //In tutorial, there is an instruction of how to show skill interface.
        //After the player follow the instruction, show the next one.
        protected override void resetCamp()
        {
            //Same as MouseControl script.
            CampManager campManager = GameObject.FindObjectOfType<CampManager>();
            Camp camp=campManager.GetCamp();
            if(camp==null) return;
            camp.ResetCampColor();
            campManager.SetCamp();            
            if (!GameObject.FindWithTag("Cost").activeInHierarchy) return;
            campManager.ShowTarget("Skill");

            //check the check point is completed or not.
            pagesControl.CheckFlag(flag);
            pagesControl.FlagComplete(flag);
        }
    }
}
