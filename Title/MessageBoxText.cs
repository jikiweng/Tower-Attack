using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Title
{
    //Attach to the message box shows in settings scene when language button is clicked.
    public class MessageBoxText : MonoBehaviour
    {
        [SerializeField] Text messageboxText = null;
        [SerializeField] Text yesText = null;
        [SerializeField] Text noText = null;

        // Start is called before the first frame update
        void Start()
        {
            //change the text with current language.
            Fader fader = GameObject.FindObjectOfType<Fader>();
            switch (fader.Language)
            {
                case "Chinese":
                    messageboxText.text = "要返回標題畫面更改語言嗎?";
                    yesText.text = "確定";
                    noText.text = "返回";
                    break;
                case "Japanese":
                    messageboxText.text =
                        "タイトル画面に戻って言語を変更しますか？";
                    yesText.text = "はい";
                    noText.text = "いいえ";
                    break;
                default:
                    messageboxText.text =
                        "Return to main menu for changing language?";
                    yesText.text = "Yes";
                    noText.text = "No";
                    break;
            }
        }
    }
}
