using UnityEngine;
using System.Collections;

namespace TowerAttack.Title
{
    //Attach to the background image for title scene.
    public class Background : MonoBehaviour 
    { 
        //this object will shows when this one is closed.
        [SerializeField] GameObject background2=null;
        //the point the picture starts.
        [SerializeField] Vector3 startPoint=new Vector3(0,0,0);
        //the direction this picture moves.
        [SerializeField] Vector3 direction=new Vector3(0,0,0);

        private void Start()
        {
            startAnimation();
        }

        //Reset the position for the image and starts to move it.
        public void startAnimation() 
        {
            transform.position=startPoint;
            StartCoroutine(moveBackground()); 
        }

        //Use leantween to do the move animation.
        private IEnumerator moveBackground()
        {
            LeanTween.move(gameObject,direction,10f);
            yield return new WaitForSeconds(10);

            //when the image reach it's end, close it and open the other one. 
            background2.SetActive(true);
            background2.GetComponent<Background>().startAnimation();
            gameObject.SetActive(false);  
        }
    }
}
