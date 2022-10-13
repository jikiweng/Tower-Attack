using UnityEngine;
using UnityEngine.UI;
using TowerAttack.Title;

namespace TowerAttack.Tutorial
{
    //Attach to the each page.
    public class ChangeText : MonoBehaviour 
    { 
        //the text will be showed when changed to the certain language.
        [SerializeField] string ChineseText;
        [SerializeField] string JapaneseText;
        [SerializeField] string EnglishText;

        private Text text;
        //the current language is saved in fader.
        private Fader fader;

        private void Awake() 
        {
            fader=GameObject.FindObjectOfType<Fader>();
            text=GetComponentInChildren<Text>();
        }

        //Set the language at the beginning.
        private void Start() 
        {
            ChangeLanguage();
        }

        //Change the text with current language.
        public void ChangeLanguage()
        {
            switch(fader.Language)
            {
                case "Chinese":
                    text.text=ChineseText;
                    break;
                case "Japanese":
                    text.text=JapaneseText;
                    break;
                default:
                    text.text=EnglishText;
                    break;
            }
        }
    }
}
