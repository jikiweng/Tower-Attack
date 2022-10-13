using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerAttack.Combat;
using TowerAttack.Stats;

namespace TowerAttack.AI
{
    //Attach to the detect areas for all towers.
    public class TowerManager : MonoBehaviour
    {
        //All soldier types this tower can products.
        //The cost of the soldiers should be arranged from small to large.
        [SerializeField] SoldierPiece[] soldierPieces = new SoldierPiece[3];
        //There are 4 spawnPoints around the tower.
        [SerializeField] SpawnPoint[] spawnPoints=new SpawnPoint[4];
        [SerializeField] string towerType="small tower";
 
        private Vector3 spawnPointPostion;        
        //The balance for this tower used to spawn soldiers.
        private int coin; 

        private void Start() 
        {
            int i=0;
            
            //the order is written in ParameterManager script.
            switch (towerType)
            {
                case "small tower":
                    i=0;
                    break;
                case "big tower":
                    i=1;
                    break;
                case "castle":
                    i=2;
                    break;
            }

            //the list will change with fader.Difficulty.
            //get the coins this tower has in this difficulty.
            coin=GameObject.FindObjectOfType<ParameterManager>().TowerCoinList[i];
        }

        void OnTriggerEnter(Collider other)
        {
            //The TowerManager will be activated by invading friend soldier.
            CombatTarget combatTarget = other.GetComponent<CombatTarget>();
            if (combatTarget==null||
            combatTarget.combatTargetType != CombatTargetType.Friend) return;

            //Get the transform component from the closest spawn point.
            SpawnPoint spawn=findNearestSpawnPoint(other.gameObject);
            spawnPointPostion=spawn.GetComponent<Transform>().position;

            //Close the detect collider. In case it may disturb the spawnpoint.
            GetComponent<SphereCollider>().enabled = false;

            //Continue spawning the soldiers until the coin runs out.
            InvokeRepeating("spawnSoldier1", 0f, soldierPieces[0].coolDownTime);
            if (soldierPieces[1] == null) return;

            InvokeRepeating("spawnSoldier2", 2f, soldierPieces[1].coolDownTime);
            if (soldierPieces[2] == null) return;

            InvokeRepeating("spawnSoldier3", 4f, soldierPieces[2].coolDownTime);
        }

        //There are 4 spawnpoints, choose the one closest to the invaders.
        private SpawnPoint findNearestSpawnPoint(GameObject other)
        {
            //Set default value.
            float min=Mathf.Infinity;
            SpawnPoint spawn=spawnPoints[0];

            //Find the one has smallest distance.
            foreach(SpawnPoint spawnPoint in spawnPoints)
            {
                float distance=Vector3.Distance(spawnPoint.GetComponent<Transform>().position,
                other.GetComponent<Transform>().position);

                if(distance<min)
                {
                    min=distance;
                    spawn=spawnPoint;
                }
            }
            return spawn;
        }

        //Spawn the cheapest soldier and substract the cost from coin.
        private void spawnSoldier1()
        {
            int cost = soldierPieces[0].cost;

            soldierPieces[0].SpawnSoldier(spawnPointPostion);
            coin -= cost;

            //CancelInvoke will cancel all the invoke method, 
            //so only when coin can not afford the cheapest one should call CancelInvoke.
            if (coin < cost) CancelInvoke();
        }

        //Spawn the second cheap soldier.
        //Return if the coin is not enough.
        private void spawnSoldier2()
        {
            int cost = soldierPieces[1].cost;
            if (coin < cost) return;

            soldierPieces[1].SpawnSoldier(spawnPointPostion);
            coin -= cost;
        }

        //Spawn the most expensive soldier.
        //Return if the coin is not enough.
        private void spawnSoldier3()
        {
            int cost = soldierPieces[2].cost;
            if (coin < cost) return;

            soldierPieces[2].SpawnSoldier(spawnPointPostion);
            coin -= cost;
        }

        public void CancelAllRespawn()
        {
            CancelInvoke("spawnSoldier1");
            if (soldierPieces[1] != null) CancelInvoke("spawnSoldier2");
            if (soldierPieces[2] != null) CancelInvoke("spawnSoldier3");
        }
    }
}
