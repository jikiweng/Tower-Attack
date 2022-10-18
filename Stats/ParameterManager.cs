using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerAttack.Title;

namespace TowerAttack.Stats
{
    //Attach to main camera.
    public class ParameterManager : MonoBehaviour
    {
        //Controls all the towers and other parameters.
        public readonly List<Tower> TowerList = new List<Tower>();
        private GameObject[] list;

        //this list is the money small tower, big tower and castle has.
        public int[] TowerCoinList=new int[3];
        [SerializeField] string difficulty="";

        //result=0:gaming result=1:win result=-1:lose
        private int result=0;
        public int GetResult(){ return result; }
        public void SetResult(int result){ this.result=result; }

        private Fader fader;

        void Awake() 
        {
            fader=GameObject.FindObjectOfType<Fader>();

            //if it is tutorial, the small tower has 200 money; big tower have 300; castlehave 400.
            if(difficulty!="")
            {
                TowerCoinList=new int[]{200,300,400};
            }
            else 
            {
                //if it is not tutorial, set the money for towers.
                difficulty=fader.Difficulty;
                switch (difficulty)
                {
                    case "Easy":
                        TowerCoinList=new int[]{300,600,900};
                        break;
                    case "Normal":
                        TowerCoinList=new int[]{500,800,1100};
                        break;
                    case "Difficult":
                        TowerCoinList=new int[]{700,1000,1500};
                        break;
                }
            }
        }

        //Put all the towers into the list so that the soldier can find the nearest one.
        void Start()
        {
            list = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject tower in list)
            {
                TowerList.Add(tower.GetComponent<Tower>());
            }            
        }

        //When a tower falls, it has to be removed from the tower list.
        //Call by CombatTarget script.
        public void RemoveTower(Tower tower)
        {
            TowerList.Remove(tower);
        }

        public void ReturnToMenu()
        {
            fader.LoadNewScene(0);
        }
    }
}
