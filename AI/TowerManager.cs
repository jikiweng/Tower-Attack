using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.AI
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField]
        SoldierPiece[] soldierPieces = new SoldierPiece[3];
        [SerializeField] SpawnPoint[] spawnPoints=new SpawnPoint[4];

        [SerializeField]
        int coin = 500;

        private Transform spawnPointTransform=null;

        void OnTriggerEnter(Collider other)
        {
            //The TowerManager will be activated by invading friend soldier.
            CombatTarget combatTarget = other.GetComponent<CombatTarget>();
            if (combatTarget==null||
            combatTarget.combatTargetType != CombatTargetType.Friend) return;

            float min=Mathf.Infinity;
            SpawnPoint spawn=spawnPoints[0];
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
            spawnPointTransform=spawn.GetComponent<Transform>();

            //isActive = true;
            GetComponent<SphereCollider>().enabled = false;

            float timeBetweenSpawnSoldier1 = soldierPieces[0].coolDownTime;
            InvokeRepeating("spawnSoldier1", 0f, timeBetweenSpawnSoldier1);
            if (soldierPieces[1] == null) return;

            float timeBetweenSpawnSoldier2 = soldierPieces[1].coolDownTime;
            InvokeRepeating("spawnSoldier2", 2f, timeBetweenSpawnSoldier2);
            if (soldierPieces[2] == null) return;

            float timeBetweenSpawnSoldier3 = soldierPieces[2].coolDownTime;
            InvokeRepeating("spawnSoldier3", 4f, timeBetweenSpawnSoldier3);
        }

        private void spawnSoldier1()
        {
            int cost = soldierPieces[0].cost;

            soldierPieces[0].SpawnSoldier(spawnPointTransform);
            coin -= cost;

            if (coin < cost) CancelInvoke();
        }

        private void spawnSoldier2()
        {
            int cost = soldierPieces[1].cost;
            if (coin < cost) return;

            soldierPieces[1].SpawnSoldier(spawnPointTransform);
            coin -= cost;
        }

        private void spawnSoldier3()
        {
            int cost = soldierPieces[2].cost;
            if (coin < cost) return;

            soldierPieces[2].SpawnSoldier(spawnPointTransform);
            coin -= cost;
        }
    }
}
