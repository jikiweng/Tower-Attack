using UnityEngine;
using UnityEngine.UI;
using TowerAttack.UI;

namespace TowerAttack.Tutorial
{
    //Attach to the Pages gameObject.
    public class PagesControl : MonoBehaviour
    {
        //this array collects all the pages.
        [SerializeField] GameObject[] pages=null;
        //the block is different from the block for pause menu.
        //so that it can make sure always a block when pause menu or tutorial shows.
        [SerializeField] GameObject block=null;

        //use to check the check point.
        private int currentflag=0;
        //use to get the index of previous page.
        private int previousPage=0;
        //use to get the index of current page.
        private int currentPage=0;
        //the audio clip for button click sound effect.
        private AudioSource buttonAudio;
        private MouseControl mouseControl;

        // Start is called before the first frame update
        void Start()
        {   
            //shoe the first page and the block object.
            showPage();
            block.SetActive(true);
            buttonAudio=GameObject.FindGameObjectWithTag("SoundEffect").GetComponent<AudioSource>();

            mouseControl=GameObject.FindObjectOfType<MouseControl>();
            mouseControl.IsMoving=false;
        }

        //When the check flag is checked or the next button is clicked, show next page.
        public void ShowNextPage()
        {
            currentPage+=1;
            showPage();
        }

        //When the back button is clicked, show previous page.
        public void ShowPreviousPage()
        {
            currentPage-=1;
            showPage();
        }

        //Close the previous page and show current page.
        private void showPage()
        {
            pages[previousPage].SetActive(false);
            //if the currentPage is over the last page, tutorial ends. close the block object.
            if(currentPage<0||currentPage>=pages.Length) 
            {
                block.SetActive(false);
                Time.timeScale=1;
                return;
            }
            
            pages[currentPage].SetActive(true);
            previousPage=currentPage;
        }

        //When the tutorial comes to a check point, close the current page but do not show next page.
        public void ClosePage()
        {
            pages[previousPage].SetActive(false);
            currentflag+=1;
            //resume the time.
            Time.timeScale = 1;
            block.SetActive(false);
            mouseControl.IsMoving=true;
        }

        //Make sure that only complete current instruction can unlock the next instruction.
        public void CheckFlag(int flag)
        {
            if(currentflag!=flag) return;
            currentflag+=1;
        }

        //Stop the time and show next flag.
        public void FlagComplete(int flag)
        {
            if(currentflag!=flag+1) return;
            ShowNextPage();   
            //stop the time.         
            Time.timeScale = 0;
            block.SetActive(true);
            mouseControl.IsMoving=false;
        }

        //play the sound effect when any button is clicked.
        public void PlayButtonAudio()
        {
            buttonAudio.Play();
        }
    }
}
