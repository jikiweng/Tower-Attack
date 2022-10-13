using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TowerAttack.AI;

namespace TowerAttack.Combat
{
    //Attach to the balance text.
    public class Balance : MonoBehaviour
    {
        [SerializeField] SoldierPiece[] soldiers=null;
        [SerializeField] GameObject[] LoseEffects=null;   
        [SerializeField] AudioSource loseMusic=null;
        public bool HasMoney = true;

        private int minCost;
        //The amount shows how much money left.
        private int balance;
        private Text balanceText;

        void Awake()
        {
            balanceText = GetComponent<Text>();
            balance = int.Parse(balanceText.text);
        }

        private void Start()
        {
            minCost=soldiers[0].cost;
            for(int i=1;i<soldiers.Length;i++)
            {
                if(soldiers[i].cost<minCost)
                    minCost=soldiers[i].cost;
            }
        }

        //The method to change balance.
        //It will be called when spawn soldiers or towers fall. One is decrease and the other is increase.
        //The bool is to check if the balance can afford to spawn a soldier.
        public bool SetBalance(int money)
        {
            //return false if the money left can not spawn this soldier.
            if(balance + money < 0) return false;

            //change the text and the money left.
            balance += money;
            balanceText.text = balance.ToString();

            if(balance<minCost) HasMoney=false;
            return true;
        }

        //Destroy the plusCoins text after 2 seconds.
        public void DestroyText()
        {
            Invoke("deleteFunc", 2f);
        }

        //Find the plusCoins gameObject and show it on the screen.
        private void deleteFunc()
        {
            GameObject plusCoins =
                GameObject.FindGameObjectWithTag("plusCoins");
            if (plusCoins != null) plusCoins.SetActive(false);
        }

        public void CheckIfLose()
        {
            Invoke("isLose",1f);
        }

        private void isLose()
        {
            SoldierFriend liveSoldier=GameObject.FindObjectOfType<SoldierFriend>();
            if(liveSoldier!=null) return;

            foreach(GameObject obj in LoseEffects)
            {
                obj.SetActive(true);
            }
            loseMusic.Play();
        }
    }
}
