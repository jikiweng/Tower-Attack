using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerAttack.AI;
using TowerAttack.Combat;

namespace TowerAttack.Core
{
    //Attach to the cost icon.
    public class SpawnSoldier : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;
        //used to control cool down time.
        [SerializeField] SpawnCoolDown spawnCoolDown=null;

        private Text costText;

        void Awake()
        {
            costText=GetComponentInChildren<Text>();
        }

        //If the soldier can be spawned, spend money→ spawn one→ reset cool down time.
        public void OnClick()
        {
            if (!spawnCoolDown.CanBeSpawn()) return;
            //Get the selected camp.
            Camp camp = FindObjectOfType<CampManager>().GetCamp();
            if (camp == null) return;

            //Spend money from balance.
            int cost = int.Parse(costText.text);
            if (!FindObjectOfType<Balance>().SetBalance(0 - cost)) return;
            
            //Spawn a soldier to spawn point.
            Vector3 spawnPoint = camp.GetComponentInChildren<SpawnPoint>().transform.position;
            soldierPiece.SpawnSoldier(spawnPoint);
            //Reset cool down time.
            spawnCoolDown.ResetCoolDownTime();
        }
    }
}
