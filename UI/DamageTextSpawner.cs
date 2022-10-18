using UnityEngine;

namespace TowerAttack.UI
{
    //Attach to the spawner under soldier gameObject.
    public class DamageTextSpawner : MonoBehaviour 
    { 
        //the prefab to spawn.
        [SerializeField] GameObject damageTextPrefab;
        //the distance above health bar.
        [SerializeField] float textPosition=2;

        //Called on damage, spawn DamageText.
        public void SpawnDamageText(float damage,Color color)
        {
            //spawn a new DamageTextand change the color into red.
            GameObject damageText=Instantiate(damageTextPrefab,transform);

            //some soldiers has the ability to recover. change the color into yellow.
            string damageToString=damage.ToString();
            if(damage<0)
            {
                damage*=-1;
                damageToString="+"+damage;
            }

            //change the text of DamageText.
            damageText.GetComponent<DamageText>().SetDamage(damageToString,textPosition+transform.position.y,color);
        }
    }
}
