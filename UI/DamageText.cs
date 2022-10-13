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
            StartCoroutine(damageTextAnimation(height));
        }

        //Damage Text will go up, become larger and transparent after spawned.
        //This animation is controled by LeanTween.
        private IEnumerator damageTextAnimation(float height) 
        {
            LeanTween.moveY(gameObject,height,displayTime);   
            LeanTween.scale(rectTransform,new Vector3(0.05f,0.05f,0.05f),displayTime);
            LeanTween.alpha(gameObject,0f,displayTime);  

            //destroy the DamageText after displaytime.
            yield return new WaitForSeconds(displayTime);
            Destroy(gameObject);       
        }
    }
}
