using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TowerAttack.Core
{
    //Attach to the camp image, used to control dragging behaviour.
    public class CampManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] GameObject spawnPrefab=null;
        //shows how many camps left.
        [SerializeField] Text remainText=null;
        //while dragging, the parent need to be change to fit the whole screen size.
        [SerializeField] Transform parentCanvasTransform=null;
        public GameObject[] costs;
        //when clicked, hides skill icon.
        public GameObject[] skills;
        
        //control the selected camp.
        private Camp camp;
        public Camp GetCamp() { return camp; }
        public void SetCamp(Camp selectedCamp = null) { this.camp = selectedCamp; }

        //tells the cost buttons are showed or not.
        private bool isShowCost = false;
        public bool GetShowCost() { return isShowCost; }
        public void SetShowCost(bool isShow) { this.isShowCost = isShow; }

        // PRIVATE STATE
        Transform spriteTransform;
        GameObject spawn;
        // CACHED REFERENCES
        CanvasGroup canvasGroup;

        // LIFECYCLE METHODS
        private void Awake()
        {
            canvasGroup=GetComponent<CanvasGroup>();
        }

        //Once upon the image clicked, spawn a same image and set the parent to screen canvas.
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            //If there is no camps left, do nothing.
            if(remainText.text=="0") return;

            //spawn the image.
            spawn = Instantiate(gameObject, GetComponent<Transform>(), true);
            spawn.GetComponent<CanvasGroup>().alpha = 0.8f;

            // Set parent.
            spriteTransform = spawn.GetComponent<Transform>();
            spriteTransform.SetParent(parentCanvasTransform, true);

            //false=>block raycast.
            canvasGroup.blocksRaycasts = false;
        }

        //Set the spawned image position to follow the mouse.
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            spriteTransform.position = eventData.position;
        }

        //Spawn a camp on the position where mouse ends to.
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            Destroy(spawn);

            //Use raycast hit to change screen position into world position.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Physics.Raycast(ray, out hit);

            //This will spawn a camp on the assigned position.
            dropItem(hit.point);

            //Open the block for raycast and substract the remain camp.
            canvasGroup.blocksRaycasts = true;
            int remain=int.Parse(remainText.text)-1;
            remainText.text=remain.ToString();
        }

        //Spawn a camp on the assigned position.
        private void dropItem(Vector3 position)
        {
            Instantiate(spawnPrefab, position, Quaternion.identity);
        }

        //This method is used to display skill icons or cost buttons.
        //This is called in Camp & MoveMap script.
        public void ShowTarget(string targetName)
        {
            //Show skill icons and hide cost buttons.
            if (targetName == "Skill")
            {
                foreach (GameObject skill in skills)
                { skill.SetActive(true); }
                foreach (GameObject cost in costs)
                { cost.SetActive(false); }
            }

            //Show cost buttons and hide skill icons.
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
