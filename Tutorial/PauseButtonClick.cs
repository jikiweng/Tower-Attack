using UnityEngine;
using TowerAttack.Title;

namespace TowerAttack.Tutorial
{
    //Attach to the language button in pause menu.
    public class PauseButtonClick : MonoBehaviour 
    {
        //used to compare with current language.
        public string ButtonName="";
        private Fader fader;
        private PauseMenu pauseMenu;

        //Only the language equals to current language will run this method.
        //The text will be set to that language.
        private void Start() 
        {
            fader=GameObject.FindObjectOfType<Fader>();
            pauseMenu=GameObject.FindObjectOfType<PauseMenu>();
            if(fader.Language==ButtonName)
                pauseMenu.SetLanguage(gameObject);
        }
    }
}
