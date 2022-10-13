using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Title
{
    //Attach to the language button in settings.
    public class SettingButtonClick : MonoBehaviour
    {
        //use to identify the language button.
        public string buttonName = ""; 

        private Fader fader;
        private SettingsControl settingsControl;

        private void Awake()
        {
            fader=GameObject.FindObjectOfType<Fader>();
            settingsControl=GameObject.FindObjectOfType<SettingsControl>();
            //only the language and difficulty matchs current parameter can set the languagr.
            if(fader.Language==buttonName) settingsControl.SetLanguage(gameObject);
            else if(fader.Difficulty==buttonName) settingsControl.SetDifficulty(gameObject);
        }

        //If the pressed language is the same with current languea, do nothing,
        //Otherwise, change the language or difficulty.
        public void ButtonClicked()
        {
            if(fader.Language==buttonName||fader.Difficulty==buttonName) return;
            if(gameObject.tag=="Language") settingsControl.LanguageButtonClicked(gameObject);
            if(gameObject.tag=="Difficulty") settingsControl.SetDifficulty(gameObject);
        }

        //Called when any of the button is clicked. Set sprite and colors.
        public void SetSprite(Sprite sprite, Color color)
        {
            gameObject.GetComponent<Image>().sprite=sprite;
            gameObject.GetComponentInChildren<Text>().color=color;
        }
    }
}
