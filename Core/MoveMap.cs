using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerAttack.UI;

namespace TowerAttack.Core
{
    //Attach to the map area.
    public class MoveMap : MonoBehaviour, IRaycastable, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Transform originTransform=null;
        //the vector records the difference between clickpoint and currentpoint.
        private Vector3 move;
        //the point where dragging starts.
        private Vector3 clickPoint;
        //the point where dragging 
        private Vector3 currentPoint;
        
        void Awake()
        {
            move=originTransform.position;
        }

        //When the map area clicked, close the cost buttons and show the skill icons.
        public void OnPointerClick(PointerEventData eventData)
        {
            //Change the selected camp color into default.
            //Then set the selected camp to null.
            CampManager campManager = GameObject.FindObjectOfType<CampManager>();
            Camp camp=campManager.GetCamp();
            if(camp==null) return;

            camp.ResetCampColor();
            campManager.SetCamp();
            
            //This will trigger the method to adjust panel display.
            if (!GameObject.FindWithTag("Cost").activeInHierarchy) return;
            campManager.ShowTarget("Skill");
        }

        //When the map area clicked, close the cost buttons and show the skill icons.
        public void OnBeginDrag(PointerEventData eventData)
        {
            clickPoint=getMousePoint(eventData.position);
            //Debug.Log("("+clickPoint.x+","+clickPoint.y+","+clickPoint.z+")");
        }

        //While dragging, move the camera with the direction of mouse.
        public void OnDrag(PointerEventData eventData)
        {
            //Cant set transform.position.x directly. Must set Vecotr3 first and then change trasnform.
            currentPoint=getMousePoint(eventData.position);
            move.x-=currentPoint.x-clickPoint.x;
            move.z-=currentPoint.z-clickPoint.z;
            originTransform.position=move;
        }

        //Reset move.
        public void OnEndDrag(PointerEventData eventData)
        {
            move=originTransform.position;
            //Debug.Log("("+currentPoint.x+","+currentPoint.y+","+currentPoint.z+")");
        }

        private Vector3 getMousePoint(Vector3 position)
        {
            // RaycastHit hit;
            // Ray ray = Camera.main.ScreenPointToRay(position);
            // Physics.Raycast(ray, out hit,LayerMask.GetMask("Maps"));
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit[] hits=Physics.RaycastAll(ray,LayerMask.GetMask("Maps"));

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast())
                    {
                        return hit.point;
                    }
                }
            }
            return clickPoint;
        }

        // private RaycastHit[] raycastAllSorted(Ray ray)
        // {
        //     //get all hits
        //     RaycastHit[] hits = Physics.RaycastAll(ray,LayerMask.GetMask("Maps"));
        //     //build array distances
        //     float[] distances = new float[hits.Length];
        //     for (int i = 0; i < hits.Length; i++)
        //     {
        //         distances[i] = hits[i].distance;
        //     }
        //     //sort the hits by distance
        //     Array.Sort(distances, hits);
        //     Array.Reverse(hits);
        //     return hits;
        // }

        public CursorType GetCursorType(){return CursorType.Select;}
        public bool HandleRaycast(){return true;}
    }
}
