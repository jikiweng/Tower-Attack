using UnityEngine;
using TowerAttack.Tutorial;
using TowerAttack.UI;

namespace TowerAttack.Core
{
    //Attach to tutorial camp.
    public class TutorialCamp : Camp
    { 
        private PagesControl pagesControl;

        //Complete fald number 1 when this script starts.
        protected override void Start()
        {
            material = GetComponentInChildren<Renderer>().material;
            mouseControl=GameObject.FindObjectOfType<MouseControl>();

            pagesControl=GameObject.FindObjectOfType<PagesControl>();
            pagesControl.CheckFlag(1);
            pagesControl.FlagComplete(1);
        }

        public override void ClickCamp()
        {
            pagesControl.CheckFlag(3);
            pagesControl.FlagComplete(3);
            
            setSelectedCamp();   
        }
    }
}
