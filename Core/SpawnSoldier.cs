using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerAttack.AI;

namespace TowerAttack.Core
{
    public class SpawnSoldier : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;
        [SerializeField] SpawnCoolDown spawnCoolDown=null;

        private Camp camp;

        public void OnClick()
        {
            if (!spawnCoolDown.CanBeSpawn()) return;
            camp = FindObjectOfType<CampManager>().GetCamp();
            if (camp == null) return;

            int cost = int.Parse(GetComponentInChildren<Text>().text);
            if (!FindObjectOfType<Balance>().SetBalance(0 - cost)) return;

            Transform spawnPoint = camp.GetComponentInChildren<SpawnPoint>().transform;
            soldierPiece.SpawnSoldier(spawnPoint);
            spawnCoolDown.ResetCoolDownTime();
        }
    }
}
