using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerAttack.Stats;
using TowerAttack.UI;

namespace TowerAttack.Combat
{
    //CombatTarget includes castles, towers and soldiers, use to get the target gameObect and take damages.
    public class CombatTarget : MonoBehaviour
    {        
        //the public event controls different behaviour for different object getting damaged.
        [SerializeField] UnityEvent onDamage=null;
        //this gameObject spawns DamageText while hurting.
        [SerializeField] DamageTextSpawner damageTextSpawner=null;

        //healthPoint equals to current health point, which will change on damaged.
        //MaxHealthPoint is the max of health point, changing when level up.
        private float healthPoint;
        public float HealthPoint { get { return healthPoint; } }
        public float MaxHealthPoint=30;

        //This bool is used to check if the target in the detect area is still alive.
        //Soldiers will move on to the next target if the previous one is dead.
        protected bool isDead = false;
        public bool IsDead { get { return isDead; } }
        
        //use for enemy soldier to catch shooter soldier.
        public bool IsShooter=false;

        //Shows this Soldier is a friend or an enemy.
        public CombatTargetType combatTargetType = CombatTargetType.Friend;

        void Awake()
        {
            //HP starts from max HP when the target is spawned.
            healthPoint=MaxHealthPoint;
        }

        //When taking damage,the remain health will be substracted by the damage.
        public void TakeDamage(float damage,Color color)
        {
            //if the soldier is dead, do nothing.
            if(isDead) return;
            if(damage<0) color=Color.yellow;
                        
            //If the remain health is smaller than 0, set the health point to 0.
            healthPoint-=damage;
            healthPoint = Mathf.Max(0, healthPoint);
            healthPoint=Mathf.Min(healthPoint,MaxHealthPoint);            
            
            //spawn DamageText.
            damageTextSpawner.SpawnDamageText(damage,color);

            //Call all the function set for the public event.
            //This will trigger the method to adjust health bar.
            onDamage.Invoke();
            
            //If the remain health is equal to 0, call "die" function.
            if (healthPoint <= 0) die();
        }

        protected virtual void die(){}
    }
}