using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TowerAttack.Core
{
    public class CampManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] GameObject spawnPrefab=null;
        [SerializeField] Text remainText=null;
        public GameObject[] costs;
        public GameObject[] skills;
        
        private Camp camp;
        public Camp GetCamp() { return camp; }
        public void SetCamp(Camp camp = null) { this.camp = camp; }

        private bool isShowCost = false;
        public bool GetShowCost() { return isShowCost; }
        public void SetShowCost(bool isShow) { this.isShowCost = isShow; }

        // PRIVATE STATE
        Transform spriteTransform;
        GameObject spawn;
        // CACHED REFERENCES
        Canvas parentCanvas;

        // LIFECYCLE METHODS
        private void Awake()
        {
            parentCanvas = FindObjectOfType<Canvas>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if(remainText.text=="0") return;

            spawn = Instantiate(gameObject, GetComponent<Transform>(), true);
            spawn.GetComponent<CanvasGroup>().alpha = 0.8f;
            // Else won't get the drop event.
            spriteTransform = spawn.GetComponent<Transform>();
            spriteTransform.SetParent(parentCanvas.GetComponent<Transform>(), true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            spriteTransform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            Destroy(spawn);

            //raycast to terrain
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Physics.Raycast(ray, out hit);

            dropItem(hit.point);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            int remain=int.Parse(remainText.text)-1;
            remainText.text=remain.ToString();
        }

        private void dropItem(Vector3 position)
        {
            Instantiate(spawnPrefab, position, Quaternion.identity);
        }

        public void ShowTarget(string targetName)
        {
            if (targetName == "Skill")
            {
                foreach (GameObject skill in skills)
                { skill.SetActive(true); }
                foreach (GameObject cost in costs)
                { cost.SetActive(false); }
            }

            else if (targetName == "Cost")
            {
                foreach (GameObject skill in skills)
                { skill.SetActive(false); }
                foreach (GameObject cost in costs)
                { cost.SetActive(true); }
            }
        }
    }
}
