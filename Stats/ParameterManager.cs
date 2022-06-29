using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.Stats
{
    //Attach to main camera.
    public class ParameterManager : MonoBehaviour
    {
        //Controls all the towers and other parameters.
        public readonly List<Tower> TowerList = new List<Tower>();
        private GameObject[] list;

        //result=0:gaming result=1:win result=-1:lose
        private int result=0;
        public int GetResult(){ return result; }
        public void SetResult(int result){ this.result=result; }

        //Put all the towers into the list so that the soldier can find the nearest one.
        void Start()
        {
            list = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject tower in list)
            {
                TowerList.Add(tower.GetComponent<Tower>());
            }
            //Debug.Log(TowerList.Count);
        }

        //When a tower falls, it has to be removed from the tower list.
        //Call by CombatTarget script.
        public void RemoveTower(Tower tower)
        {
            TowerList.Remove(tower);
        }
    }
}
