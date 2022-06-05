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
    }
}
