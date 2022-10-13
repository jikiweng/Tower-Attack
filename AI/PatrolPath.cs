using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerAttack.AI
{
    //Attach to the patrol path who child some nudes.
    public class PatrolPath : MonoBehaviour
    {
        //[SerializeField] float waypointGizmoRadius = 0.1f;
        // private void OnDrawGizmos()
        // {
        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         int j = GetNextIndex(i);

        //         Gizmos.DrawSphere(GetCurrentWaypoint(i), waypointGizmoRadius);
        //         Gizmos.DrawLine(GetCurrentWaypoint(i), GetCurrentWaypoint(j));
        //     }
        // }

        //Get the index for the next nude.
        public int GetNextIndex(int i)
        {
            //The index will be 0 if current nude is the last nude.
            //Otherwise just simply add the index with 1.
            return (i == transform.childCount - 1) ? 0 : i+1;
        }

        //Get the position for the current nude.
        public Vector3 GetCurrentWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}