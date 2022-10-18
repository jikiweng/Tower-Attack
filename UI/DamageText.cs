using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TowerAttack.UI
{
    //Attach to DamageText prefab.
    public class DamageText : MonoBehaviour 
    { 
        //the time DamageText exists.
        [SerializeField] float displayTime=0;
        private Text damageText;
        private RectTransform rectTransform;

        private void Awake() 
        {
            damageText=GetComponent<Text>();
            rectTransform=GetComponent<RectTransform>();
        }

        //This will changed the text contents and parameters.
        public void SetDamage(string damage,float height,Color color)
        {
            damageText.text=damage;
            damageText.color=color;

            Vector3 scale=new Vector3(0.05f,0.05f,0.05f);
            if(color==new Color(255,0,255))
            {
                scale=new Vector3(0.06f,0.06f,0.06f);
                damageText.fontStyle=FontStyle.Bold;
            }

            StartCoroutine(damageTextAnimation(height,scale));
        }

        //Damage Text will go up, become larger and transparent after spawned.
        //This animation is controled by LeanTween.
        private IEnumerator damageTextAnimation(float height,Vector3 scale) 
        {
            LeanTween.moveY(gameObject,height,displayTime);   
            LeanTween.scale(rectTransform,scale,displayTime);
            LeanTween.alpha(gameObject,0f,displayTime);  

            //destroy the DamageText after displaytime.
            yield return new WaitForSeconds(displayTime);
            Destroy(gameObject);       
        }
    }
}
