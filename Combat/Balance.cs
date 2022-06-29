using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Combat
{
    //Attach to the balance text.
    public class Balance : MonoBehaviour
    {
        //The amount shows how much money left.
        private int balance;
        private Text balanceText;
        
        void Awake()
        {
            balanceText = GetComponent<Text>();
            balance = int.Parse(balanceText.text);
        }

        //The method to change balance.
        //It will be called when spawn soldiers and towers fall. One is minus and the other is plus.
        public bool SetBalance(int money)
        {
            //The bool is to check if the balance can afford to spawn a soldier.
            if(balance+money<0) return false;

            balance += money;
            balanceText.text = balance.ToString();
            return true;
        }
    }
}
