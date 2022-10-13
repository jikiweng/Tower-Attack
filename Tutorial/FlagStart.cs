using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.Tutorial
{
    //Attach to the skill 2 button and blue tower. Check the flag number 9 and number 11.
    public class FlagStart : MonoBehaviour
    {
        [SerializeField] int flag=0;
        private PagesControl pagesControl;

        private void Start() 
        {
            pagesControl=GameObject.FindObjectOfType<PagesControl>();
            pagesControl.CheckFlag(flag);
            pagesControl.FlagComplete(flag);
        }
    }
}