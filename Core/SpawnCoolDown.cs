using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerAttack.AI;

namespace TowerAttack.Core
{
    public class SpawnCoolDown : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;
        [SerializeField] Image filledImage=null;

        private float timeSinceLastClick = Mathf.Infinity;
        private float coolDownTime;

        private void Awake() 
        {
            coolDownTime=soldierPiece.coolDownTime;
        }

        public bool CanBeSpawn()
        {
            return (timeSinceLastClick >= coolDownTime) ;
        }

        public void ResetCoolDownTime()
        {
            timeSinceLastClick=0f;
        }

        private void Update()
        {
            if (timeSinceLastClick >= coolDownTime) return;
            timeSinceLastClick += Time.deltaTime;
            filledImage.fillAmount = timeSinceLastClick / coolDownTime;
        }
    }
}
