using UnityEngine;
using TowerAttack.Combat;

namespace TowerAttack.Tutorial
{
    //Attach to main camera.
    public class ShowPauseMenu : MonoBehaviour 
    { 
        [SerializeField] GameObject pauseMenu=null;
        //includes panel and balance.
        [SerializeField] GameObject[] panels=null;
        //the block is different from the block for tutorial. 
        [SerializeField] GameObject block=null;

        private void Update() 
        {
            if(pauseMenu==null) return;

            //stop the time and show pause menu when Esc key pressed.
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                Time.timeScale=0;
                block.SetActive(true);
                //change the text in pause menu with current language.
                pauseMenu.GetComponent<PauseMenu>().ChangeLanguage();
            }         

            //close/open the panel and balance when X key pressed.
            if(Input.GetKeyDown(KeyCode.X))
            {
                //if balance is showed, then close it. close it under the other situation.
                Balance balance=GameObject.FindObjectOfType<Balance>();
                bool isShow=(balance==null)?true:false;
                ShowPanel(isShow);                
            }   
        }

        //close/open the panel and balance.
        public void ShowPanel(bool isShow)
        {
            foreach (GameObject item in panels)
            {
                item.SetActive(isShow);                    
            }
        }
    }
}
