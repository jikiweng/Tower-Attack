using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerAttack.UI;

namespace TowerAttack.Core
{
    //Attach to the camp image, used to control dragging behaviour.
    public class CampManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] GameObject spawnPrefab=null;
        //shows how many camps left.
        [SerializeField] Text remainText=null;
        [SerializeField] Transform spawnParent=null;
        //while dragging, the parent need to be change to fit the whole screen size.

        public GameObject[] costs;
        //when clicked, hides skill icon.
        public GameObject[] skills;

        // PRIVATE STATE
        private Camera cam;
        private NavMeshSurface _surface;
        private GameObject spawn;
        // CACHED REFERENCES
        private CanvasGroup canvasGroup;
        private Material material;
        private MouseControl mouseControl;
        
        //control the selected camp.
        private Camp camp;
        public Camp GetCamp() { return camp; }
        public void SetCamp(Camp selectedCamp = null) { this.camp = selectedCamp; }

        //tells the cost buttons are showed or not.
        private bool isShowCost = false;
        public bool GetShowCost() { return isShowCost; }
        public void SetShowCost(bool isShow) { this.isShowCost = isShow; }

        // LIFECYCLE METHODS
        private void Awake()
        {
            canvasGroup=GetComponent<CanvasGroup>();
            _surface=GameObject.FindObjectOfType<NavMeshSurface>();
            mouseControl=GameObject.FindObjectOfType<MouseControl>();
            cam=Camera.main;
        }

        //Once upon the image clicked, spawn a same image and set the parent to screen canvas.
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            //If there is no camps left, do nothing.
            if(remainText.text=="0") return;

            //stop the map from moving.
            mouseControl.IsMoving=false;

            //spawn the image.
            spawn = Instantiate(spawnPrefab, transform.position, Quaternion.identity,spawnParent);
            material=spawn.GetComponentInChildren<Renderer>().material;
            //material.SetColor("_Color", new Color(0,0,0,180));

            //false=>block raycast.
            canvasGroup.blocksRaycasts = false;
        }

        //Set the spawned image position to follow the mouse.
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            
            //Use raycast hit to change screen position into world position.
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(eventData.position);
            if(!Physics.Raycast(ray,out hit,100f,LayerMask.GetMask("Default"))) return;
            
            //if(hit==null) return;
            if(hit.transform.tag!="Ground") material.SetColor("_Color", Color.red);
            else material.SetColor("_Color", Color.white);

            spawn.transform.position=hit.point;
        }

        //Spawn a camp on the position where mouse ends to.
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (remainText.text == "0") return;
            
            //Open the block for raycast.
            canvasGroup.blocksRaycasts = true;
            mouseControl.IsMoving=true;

            //This will spawn a camp on the assigned position.
            if(material.color==Color.red)
            {
                Destroy(spawn);
                return;
            }

            spawn.GetComponent<Camp>().enabled=true;

            //Substract the remain camp
            int remain=int.Parse(remainText.text)-1;
            remainText.text=remain.ToString();

            _surface.BuildNavMesh();
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
