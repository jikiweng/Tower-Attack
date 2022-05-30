using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerAttack.UI
{
    public class MouseControl : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType Type;
            public Texture2D texture;
            public Vector2 hotSpot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        //[SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;

        void Update()
        {
            if (InteractWithUI()) return;
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            //SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //SetCursor(CursorType.Drag);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = raycastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable raycastable = hit.transform.GetComponent<IRaycastable>();
                if (raycastable == null) continue;
                if (raycastable.HandleRaycast())
                {
                    //SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }
            return false;
        }

        private RaycastHit[] raycastAllSorted()
        {
            //get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            //build array distances
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            //sort the hits by distance
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = getCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotSpot, CursorMode.Auto);
        }

        private CursorMapping getCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.Type == type)
                    return mapping;
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
