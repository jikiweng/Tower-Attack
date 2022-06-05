using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.Stats
{
    //Attach to all towers.
    public class Tower : MonoBehaviour
    {
        //Used for detecting a tower and adjusting the distance.
        [SerializeField] float radius = 1f;
        //Only used for other script. Do not set any value.
        public Transform towerTransform=null;
        
        void Awake()
        {
            towerTransform=GetComponent<Transform>();
        }
        
        //The Tower is usually too big for the soldier to reach the position.
        //So this method allows soldiers to attck the tower right besides it. 
        public float GetDistance(float distance)
        {
            return distance - radius;
        }
    }
}
