using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TowerAttack.Stats;
using TowerAttack.AI;

namespace TowerAttack.Combat
{
    public class CombatTargetTower : CombatTarget
    { 
        //the tower on the same place, cover on the enemy tower.
        [SerializeField] GameObject replaceTower=null;
        //the money rewarded after defeat the tower.
        [SerializeField] int reward=300;
        //use to change the text.
        [SerializeField] Text plusCoins=null;
        [SerializeField] AudioSource coinSound=null;
        [SerializeField] AudioSource towerFallSound=null;

        //When a tower falls, change the tower into different color.
        //Also, the tower should be removed in the tower list.
        protected override void die()
        {            
            towerFallSound.Play();
            //remove this tower from towerlist.
            Tower tower = GetComponent<Tower>();
            GameObject.FindObjectOfType<ParameterManager>().RemoveTower(tower);

            //stop spawning new enemy after defeated.
            TowerManager towerManager=GetComponentInChildren<TowerManager>();
            towerManager.CancelAllRespawn();
            isDead=true;            
            
            //different tower has different reward. Set the text equals to the number. 
            plusCoins.gameObject.SetActive(true);
            plusCoins.text="+"+reward;

            //the balance should be updated after rewarded.
            Balance balance=FindObjectOfType<Balance>();
            balance.SetBalance(reward);
            balance.DestroyText();
            coinSound.Play();

            //Turn this tower into different color.
            if (replaceTower != null)
            {
                replaceTower.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}