using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Title
{
    //Attach to the main camera in settings scene.
    public class SettingsControl : MonoBehaviour 
    {
        [SerializeField] Text languageText=null;
        [SerializeField] Text difficultyText=null;
        [SerializeField] Text easyText=null;
        [SerializeField] Text normalText=null;
        [SerializeField] Text hardText=null;        
        [SerializeField] Text volumeText=null;

        [SerializeField] Sprite unclicked=null;
        [SerializeField] Sprite clicked=null;
        [SerializeField] GameObject messagebox=null;
        [SerializeField] Slider slider=null;

        private Fader fader;
        private GameObject selectedLanguage;
        private GameObject currentLanguage;
        private GameObject selectedDifficulty;
        private AudioSource buttonAudio;

        private void Start() 
        {
            fader=GameObject.FindObjectOfType<Fader>();
            switch(fader.Language)
            {
                case "Chinese":
                    languageText.text="語言";
                    difficultyText.text="難度";
                    easyText.text="簡單";
                    normalText.text="普通";
                    hardText.text="困難";
                    volumeText.text="音量";
                    break;
                case "Japanese":
                    languageText.text="言語";
                    difficultyText.text="難易度";
                    easyText.text="簡単";
                    normalText.text="普通";
                    hardText.text="難しい";
                    volumeText.text="音量";
                    break;
                default:
                    languageText.text="Language";
                    difficultyText.text="Difficulty";
                    easyText.text="Easy";
                    normalText.text="Normal";
                    hardText.text="Hard";
                    volumeText.text="Volume";
                    break;
            }   

            buttonAudio=GameObject.FindGameObjectWithTag("SoundEffect").GetComponent<AudioSource>();
            slider.value=fader.volume;
        }

        //Called when the language buttonis are clicked. 
        //The message box is check if the player wants to back or stay in this scece
        public void LanguageButtonClicked(GameObject language)
        {
            currentLanguage=language;
            messagebox.SetActive(true);
        }

        //No buttons means do not change the language settings.
        public void NoButton()
        {
            messagebox.SetActive(false);
        }

        //Change current language and move back to title scene.
        public void YesButton()
        {
            fader.Language=currentLanguage.GetComponent<SettingButtonClick>().buttonName;
            SetLanguage(currentLanguage);
            ReturnToMenu();
        }
        
        //Reset the previous language button and change the color for new language button.
        public void SetLanguage(GameObject language)
        {
            if(selectedLanguage!=null)
                selectedLanguage.GetComponent<SettingButtonClick>().SetSprite(unclicked,new Color32(255,230,180,255));
            language.GetComponent<SettingButtonClick>().SetSprite(clicked,new Color32(150,150,150,255));
            selectedLanguage=language;            
        }

        //Reset the previous difficulty button and change the color for new difficulty button.
        public void SetDifficulty(GameObject difficulty)
        {
            if(selectedDifficulty!=null)
            {
                selectedDifficulty.GetComponent<SettingButtonClick>().SetSprite(unclicked,new Color32(255,230,180,255));
                fader.Difficulty=difficulty.GetComponent<SettingButtonClick>().buttonName;
            }
            difficulty.GetComponent<SettingButtonClick>().SetSprite(clicked,new Color32(150,150,150,255));
            selectedDifficulty=difficulty;
        } 

        public void ReturnToMenu()
        {
            fader.LoadNewScene(0);
        }

        public void PlayButtonAudio()
        {
            buttonAudio.Play();
        }

        //When the value for the slider is changed, set the volume for all sudio sourde
        public void ChangeVolume()
        {
            AudioSource[] audios=GameObject.FindObjectsOfType<AudioSource>();
            foreach(AudioSource audio in audios)
            {
                audio.volume=slider.value;
            }
            fader.volume=slider.value;
        }
    }
}