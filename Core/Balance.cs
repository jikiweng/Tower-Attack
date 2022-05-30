using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerAttack.Core
{
    public class Balance : MonoBehaviour
    {
        private int balance;
        private Text balanceText;
        // Start is called before the first frame update
        void Awake()
        {
            balanceText = GetComponent<Text>();
            balance = int.Parse(balanceText.text);
        }

        public bool SetBalance(int money)
        {
            if(balance+money<0) return false;

            balance += money;
            balanceText.text = balance.ToString();
            return true;
        }
    }
}
