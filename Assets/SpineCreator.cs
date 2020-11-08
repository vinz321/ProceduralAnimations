using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpineCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public FollowLook currentSpine;
    List<FollowLook> spine=new List<FollowLook>();
    public GameObject defaultSpine;
    public bool testing=false;
    bool editing=false;
    Vector3 axisScale=Vector3.zero;
    void Start()
    {   
        spine.Add(currentSpine);
    }
    public void switchTesting()
    {
        testing=!testing;
    }
    public void addPiece(){
        spine.Add(Instantiate(defaultSpine,currentSpine.tail,currentSpine.transform.rotation,transform).GetComponent<FollowLook>());
        currentSpine=spine[spine.Count-1];
    }
    public void removePiece(){
        spine.Remove(currentSpine);
        Destroy(currentSpine.gameObject);
        currentSpine=spine[spine.Count-1];
    }
    // Update is called once per frame
    void Update()
    {
        if(!testing){
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                axisScale=Vector3.right;
            }
            if(Input.GetKeyDown(KeyCode.Alpha2)){
                axisScale=Vector3.up;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3)){
                axisScale=Vector3.forward;
            }
            if(Input.GetKeyDown(KeyCode.Alpha4)){
                    axisScale=Vector3.one;
                }
            
            if(!Creator.limbStarted){
                if(Input.GetMouseButtonDown(0) && !IsMouseOverUI()){
                    
                    Ray ray1=Camera.main.ScreenPointToRay(Input.mousePosition);
                    float t=-ray1.origin.x/ray1.direction.x;
                    Vector3 p=new Vector3(0,ray1.direction.y*t+ray1.origin.y,ray1.direction.z*t+ray1.origin.z);
                    spine.Add(Instantiate(defaultSpine,currentSpine.tail,currentSpine.transform.rotation,transform).GetComponent<FollowLook>());
                    currentSpine=spine[spine.Count-1];
                    editing=true;
                }
                if(Input.GetMouseButton(0) && editing){
                    Ray ray1=Camera.main.ScreenPointToRay(Input.mousePosition);
                    float t=-ray1.origin.x/ray1.direction.x;
                    Vector3 p=new Vector3(0,ray1.direction.y*t+ray1.origin.y,ray1.direction.z*t+ray1.origin.z);
                    currentSpine.length=Vector3.Distance(spine[spine.Count-2].tail,p);
                    currentSpine.transform.forward=-(currentSpine.head-p).normalized;
                }
                if(Input.GetMouseButtonUp(0) || Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),100,1<<5))
                    editing=false;
                currentSpine.transform.Rotate(currentSpine.transform.right,Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                foreach(FollowLook s in spine){
                    s.transform.position+=new Vector3(0,Input.GetAxis("Vertical")*Time.deltaTime*6,0);
                }
                currentSpine.transform.localScale-=axisScale*Input.GetAxis("MouseScrollWheel")*Time.deltaTime*60;
                currentSpine.length=currentSpine.transform.localScale.z;
            }
        }
    }

    public bool IsMouseOverUI(){
        PointerEventData pointer=new PointerEventData(EventSystem.current);
        pointer.position=Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer,results);
        return results.Count>0;
    }
}
