using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public FollowLook currentSpine;
    List<FollowLook> spine=new List<FollowLook>();
    public GameObject defaultSpine;
    public bool testing=false;
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
                currentSpine.transform.Rotate(currentSpine.transform.right,Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                foreach(FollowLook s in spine){
                    s.transform.position+=new Vector3(0,Input.GetAxis("Vertical")*Time.deltaTime*6,0);
                }
                currentSpine.transform.localScale-=axisScale*Input.GetAxis("MouseScrollWheel")*Time.deltaTime*50;
                currentSpine.length=currentSpine.transform.localScale.z;
            }
        }
    }
}
