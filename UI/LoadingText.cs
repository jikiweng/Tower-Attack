using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.UI
{
    //Attach to loading text gameObject.
    //”Loading”➔”Loading.”➔”Loading..”➔”Loading...”
    public class LoadingText : MonoBehaviour 
    {  
        [SerializeField] float waitingTime=0.5f;

        //when num=3, the text should be reset.
        private int num=0;
        private float timeSinceLastUpdate=0f;
        private Text loadingText=null;

        private void Start() 
        {
            loadingText=GetComponent<Text>();            
        }

        private void FixedUpdate() 
        {
            timeSinceLastUpdate+=Time.fixedDeltaTime;

            //trigger the method when passing time is over waitingTime.
            if(timeSinceLastUpdate>=waitingTime)
            {
                //reset the passing time.
                timeSinceLastUpdate=0f;

                //reset the text when num reach 3.
                if(num==3)
                {
                    loadingText.text="Loading";
                    num=0;
                }
                //otherwise simply add a "." behind.
                else
                {
                    loadingText.text+=" .";
                    num+=1;
                }
            }
        }
    }
}
