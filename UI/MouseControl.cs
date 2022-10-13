using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerAttack.Core;

namespace TowerAttack.UI
{
    //Attach to main camera.
    public class MouseControl : MonoBehaviour
    {
        // [System.Serializable]
        // struct CursorMapping
        // {
        //     public CursorType Type;
        //     public Texture2D texture;
        //     public Vector2 hotSpot;
        // }

        // [SerializeField] CursorMapping[] cursorMappings = null;

        //the vector records the difference between clickpoint and currentpoint.
        private Vector3 move;
        //the point where dragging starts.
        private Vector3 clickPoint;
        //the mouse position while dragging.
        private Vector3 currentPoint;
        private RaycastHit hit;
        //main camera.
        private Camera cam;

        //is false while dragging the camp on to the map. 
        public bool IsMoving=false;
        private bool isClicking=false;

        void Awake() 
        {
            cam=Camera.main;            
        }

        void Update()
        {
            //When do a right click, close the cost buttons and show the skill icons.
            if(Input.GetMouseButtonDown(1))
            {               
                resetCamp();
                return;
            }

            //while dragging the camp on to the map, close the map moving function.
            if(!IsMoving) return;
            
            //when do a left click on a camp, reset the camp.
            //otherwise move the map.
            if(Input.GetMouseButtonDown(0)) 
            {
                if (InteractWithComponent()) return;
                if (!InteractWithMovement()) return;
            }
            
            //While dragging, move the camera with the direction of mouse.
            if(Input.GetMouseButton(0))
            {
                if(isClicking||!getMousePoint("Maps")) return;

                //record the mouse position.
                currentPoint=hit.point;
                //Cant set transform.position.x directly. Must set Vecotr3 first and then change trasnform.
                move.x-=(currentPoint.x-clickPoint.x)/10;
                move.z-=(currentPoint.z-clickPoint.z)/50;
                transform.position=move;
            }

            if(Input.GetMouseButtonUp(0))
            {
                isClicking=false;
            }

            //SetCursor(CursorType.None);
        }

        //Triggered when any camp is clicked.
        //Set the clicked camp the selected camp and reset the previous one.
        private bool InteractWithComponent()
        {
            if(!getMousePoint("Items")) return false;
            
            hit.transform.GetComponent<Camp>().ClickCamp();
            isClicking=true;
            return true;
        }

        //Move the main camera.
        private bool InteractWithMovement()
        {
            //Reset move.
            move=transform.position;
            if(!getMousePoint("Maps")) return false;

            //record the start position.
            clickPoint=hit.point;
            return true;
        }

        //The method can catch the desired gameObject on particular layer.
        private bool getMousePoint(string layer)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray,out hit,100f,LayerMask.GetMask(layer));
        }

        //Reset the selected camp. Change the selected camp color into default.
        //Then set the selected camp to null.
        protected virtual void resetCamp()
        {
            //get the selectedCamp.
            CampManager campManager = GameObject.FindObjectOfType<CampManager>();
            Camp camp=campManager.GetCamp();
            if(camp==null) return;

            //reset the selected camp's color.
            camp.ResetCampColor();
            campManager.SetCamp();
            
            //after do a right click, the cost interface should be changed into skill interface. 
            if (!GameObject.FindWithTag("Cost").activeInHierarchy) return;
            campManager.ShowTarget("Skill");
        }

        // private void SetCursor(CursorType type)
        // {
        //     CursorMapping mapping = getCursorMapping(type);
        //     Cursor.SetCursor(mapping.texture, mapping.hotSpot, CursorMode.Auto);
        // }

        // private CursorMapping getCursorMapping(CursorType type)
        // {
        //     foreach (CursorMapping mapping in cursorMappings)
        //     {
        //         if (mapping.Type == type)
        //             return mapping;
        //     }
        //     return cursorMappings[0];
        // }
    }
}
