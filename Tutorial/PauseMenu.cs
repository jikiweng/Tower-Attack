using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TowerAttack.Title;

namespace TowerAttack.Tutorial
{
    //Attach to the pause menu.
    public class PauseMenu : MonoBehaviour 
    { 
        [SerializeField] Text resumeText=null;    
        [SerializeField] Text backToMenuText=null;
        [SerializeField] Slider slider=null;
        //unclicked button image.
        [SerializeField] Sprite unclicked=null;
        //clicked button image.
        [SerializeField] Sprite clicked=null;
        [SerializeField] GameObject block=null;

        private Fader fader;
        private GameObject selectedLanguage;

        private void Awake() 
        {
            fader=GameObject.FindObjectOfType<Fader>(); 
            slider.value=fader.volume;
        }

        //Change the language of all text into current language.
        public void ChangeLanguage()
        {
            switch(fader.Language)
            {
                case "Chinese":
                    resumeText.text="返回遊戲";
                    backToMenuText.text="返回標題畫面";
                    break;
                case "Japanese":
                    resumeText.text="ゲームに戻る";
                    backToMenuText.text="タイトルに戻る";
                    break;
                default:
                    resumeText.text="Resume";
                    backToMenuText.text="Back to menu";
                    break;
            }   
        }

        //Resume the time before load the title scene, or else game will be freezed.
        public void ReturnToMenu()
        {
            Time.timeScale=1;
            fader.LoadNewScene(0);
        }

        //When any of the language button is clicked, set the selected language as current language. 
        //The image of the button has to be changed as well.
        public void SetLanguage(GameObject language)
        {
            //change the image of the button.
            language.GetComponent<Image>().sprite=clicked;
            language.GetComponentInChildren<Text>().color=new Color32(150,150,150,255);

            //if the previous language is not null, then the image and color should be changed.
            if(selectedLanguage!=null)
            {
                selectedLanguage.GetComponent<Image>().sprite=unclicked;
                selectedLanguage.GetComponentInChildren<Text>().color=new Color32(255,230,180,255);
                fader.Language=language.GetComponent<PauseButtonClick>().ButtonName; 
            }            
            selectedLanguage=language;
            
            //change the language of opened tutorial page.
            ChangeText changeText=GameObject.FindObjectOfType<ChangeText>(); 
            if(changeText!=null) changeText.ChangeLanguage();        
        }

        //When the value for the slider is changed, change the volume of all audio sources.
        public void ChangeVolume()
        {
            AudioSource[] audios=GameObject.FindObjectsOfType<AudioSource>();
            foreach(AudioSource audio in audios)
            {
                audio.volume=slider.value;
            }
            fader.volume=slider.value;
        }

        //Resume the time and back to the game scene.
        public void Resume()
        {
            Time.timeScale=1;
            block.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
