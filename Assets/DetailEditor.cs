using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailEditor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject defaultPiece;
    FollowLook current;
    Transform currentParent;
    Vector3 axisScale=Vector3.one;
    RaycastHit hit;
    void Start()
    {
        
    }
    void parent(){
        if(current!=null){
            Vector3 scale=current.transform.localScale;
            current.transform.parent=currentParent;
            current.transform.localScale=new Vector3(scale.x/currentParent.localScale.x,
                                                        scale.y/currentParent.localScale.y,
                                                        scale.z/currentParent.localScale.z);
        }
    }
    void OnDisable(){
        parent();
    }
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,100,3<<8)){
            parent();
            current=Instantiate(defaultPiece,hit.point,Quaternion.identity).GetComponent<FollowLook>();
            currentParent=hit.collider.transform.parent;
        }
        if(current!=null){
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
                
            current.transform.Rotate(Vector3.up,Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
            current.transform.Rotate(current.transform.right,Input.GetAxis("Vertical")*Time.deltaTime*50,Space.World);
            current.transform.localScale-=axisScale*Input.GetAxis("MouseScrollWheel")*Time.deltaTime*50;
            current.length=current.transform.localScale.z;
        }
    }
}
