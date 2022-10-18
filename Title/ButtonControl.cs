using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Title
{
    //Attach to the main camera.
    public class ButtonControl : MonoBehaviour
    {
        [SerializeField] Text tutorialButton=null;
        [SerializeField] Text newGameButton=null;
        [SerializeField] Text settingsButton=null;
        [SerializeField] Text quitButton=null;

        private Fader fader;
        private AudioSource buttonAudio;

        private void Start() 
        {
            fader=GameObject.FindObjectOfType<Fader>();
            buttonAudio=GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
            
            //change the text with current language.
            switch(fader.Language)
            {
                case "Chinese":
                    tutorialButton.text="開始教學";
                    newGameButton.text="新遊戲";
                    settingsButton.text="設定";
                    quitButton.text="離開遊戲";
                    break;
                case "Japanese":
                    tutorialButton.text="チュートリアル";
                    newGameButton.text="ゲームスタート";
                    settingsButton.text="設定";
                    quitButton.text="ゲーム終了";
                    break;
                default:
                    tutorialButton.text="Tutorial";
                    newGameButton.text="New Game";
                    settingsButton.text="Settings";
                    quitButton.text="Quit";
                    break;
            }
        }

        //Move to tutorial scene.
        public void TutorialButton()
        {
            fader.LoadNewScene(3);
        }

        //Move to new game scene.
        public void NewGameButton()
        {
            fader.LoadNewScene(1);
        }

        //Move to settings scene.
        public void SettingsButton()
        {
            fader.LoadNewScene(2);
        }

        //Stop the game.
        public void QuitGame()
        {
            Application.Quit();
        }

        //Play the sound effect.
        public void PlayButtonAudio()
        {
            buttonAudio.Play();
        }
    }
}
