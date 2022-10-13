using UnityEngine;
using TowerAttack.Stats;
using TowerAttack.AI;
using TowerAttack.Tutorial;

namespace TowerAttack.Combat
{
    public class CombatTargetCastle : CombatTarget
    { 
        //this gameObject is used to block player's operation.
        [SerializeField] GameObject block=null;
        //this gameObject shows fireworks, stage clear text and return button.
        [SerializeField] GameObject[] stageClear=null;        
        [SerializeField] AudioSource winMusic=null;
        
        //When the castle falls, game ends.
        protected override void die()
        {
            //result 1 means player wins.
            FindObjectOfType<ParameterManager>().SetResult(1);
            block.SetActive(true);

            GameObject.FindGameObjectWithTag("BGM").SetActive(false);
            winMusic.Play();

            foreach (GameObject item in stageClear)
            {
                item.SetActive(true);                
            }

            //play win animation for friend soldiers.
            SoldierFriend[] soldiers=GameObject.FindObjectsOfType<SoldierFriend>();
            foreach(SoldierFriend soldier in soldiers)
            {
                soldier.Win();
            }

            //close the panel and stop pauseMenu manipulation.            
            ShowPauseMenu showPauseMenu=GameObject.FindObjectOfType<ShowPauseMenu>();
            showPauseMenu.ShowPanel(false);
            showPauseMenu.enabled=false;
        }
    }
}