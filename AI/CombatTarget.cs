using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerAttack.Stats;

namespace TowerAttack.AI
{
    //CombatTarget includes Tower and Soldiers, decide which one canbe attacked.
    //Attach to all soldiers and towers.
    public class CombatTarget : MonoBehaviour
    {        
        //Use the public event to control different behaviour for different object.
        //All the behaviour is written below.
        [SerializeField] UnityEvent onDie=null;
        [SerializeField] UnityEvent onDamage=null;
        //If the Tower falls, change the tower into this Tower with different color.
        [SerializeField] GameObject replaceTower=null;
        [SerializeField] int reward=300;

        //healthPoint equals to current health point, which will change on damaged.
        //MaxHealthPoint is the max of health point, changing when level up.
        private float healthPoint;
        public float HealthPoint { get { return healthPoint; } }
        public float MaxHealthPoint=30;

        //This bool is used to check if the target in the detect area is still alive.
        //Soldiers will move on to the next target if the previous one is dead.
        private bool isDead = false;
        public bool IsDead { get { return isDead; } }
        //Shows this Soldier is a friend or an enemy.
        public CombatTargetType combatTargetType = CombatTargetType.Friend;
        public Transform targetTransform=null;

        void Awake()
        {
            healthPoint=MaxHealthPoint;
            targetTransform=GetComponent<Transform>();
        }

        //When taking damage,the remain health will be substracted by the damage.
        //If the remain health is smaller than 0, set the health point to 0.
        //If the remain health is equal to 0, call "die" function.
        public void TakeDamage(float damage)
        {
            healthPoint = Mathf.Max(0, healthPoint -= damage);
            //This will trigger the method to adjust health bar.
            onDamage.Invoke();
            if (healthPoint <= 0) die();
        }

        //Call all the function set for the public event.
        private void die()
        {
            onDie.Invoke();
        }

        //When a soldier dies, play the die animation and destroy the gameobject after that.
        public void SoldierDie()
        {
            GetComponent<Animator>().SetTrigger("IsDie");
            Invoke("destroyObject", 2);
        }

        //Create a new function for Invoke, so that there can be a delay after the soldier die.
        private void destroyObject()
        {
            Destroy(gameObject);
        }

        //When a tower falls, change the tower into different color.
        //Also, the tower should be removed in the tower list.
        public void TowerDie()
        {
            //Remove this tower
            Tower tower = GetComponent<Tower>();
            GameObject.FindObjectOfType<ParameterManager>().RemoveTower(tower);

            //Turn this tower into different color.
            if (replaceTower != null)
            {
                replaceTower.SetActive(true);
                gameObject.SetActive(false);
            }

            //The player can get the reward after defeat a tower.
            FindObjectOfType<Balance>().SetBalance(reward);
        }

        public void CastleDie()
        { }
    }
}