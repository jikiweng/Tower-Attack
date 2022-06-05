using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerAttack.AI;

namespace TowerAttack.Core
{
    //Attach to the soldier images.
    public class SpawnCoolDown : MonoBehaviour
    {
        [SerializeField] SoldierPiece soldierPiece=null;
        //The filled bright image is in front of the black image.
        [SerializeField] Image filledImage=null;

        private float timeSinceLastClick = Mathf.Infinity;
        private float coolDownTime;

        private void Awake() 
        {
            coolDownTime=soldierPiece.coolDownTime;
        }

        //This method is called in SpawnSoldier script to check if the soldier can be spawned.
        public bool CanBeSpawn()
        {
            return (timeSinceLastClick >= coolDownTime) ;
        }

        //This method is called in SpawnSoldier script after spawn a soldier.
        public void ResetCoolDownTime()
        {
            timeSinceLastClick=0f;
        }

        //The filled bright image will graduatedly fill up.
        private void Update()
        {
            if (timeSinceLastClick >= coolDownTime) return;
            timeSinceLastClick += Time.deltaTime;
            filledImage.fillAmount = timeSinceLastClick / coolDownTime;
        }
    }
}
