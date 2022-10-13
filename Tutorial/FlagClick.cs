using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.Tutorial
{
    //Attach to tutorial camp, use the check flag number 3.
    public class FlagClick : MonoBehaviour,IPointerClickHandler 
    {
        [SerializeField] int flag=7;
        private PagesControl pagesControl;

        private void Start() 
        {
            pagesControl=GameObject.FindObjectOfType<PagesControl>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            pagesControl.CheckFlag(flag);
            pagesControl.FlagComplete(flag);
        }
    }
}