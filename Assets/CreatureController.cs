using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    // Start is called before the first frame update
    CharacterController cc;
    public Vector3 speed;
    bool testPhase=false;
    void Start()
    {
        cc=GetComponent<CharacterController>();
    }
    public void switchTesting(){
        if(testPhase){
            testPhase=false;
            cc.enabled=false;
            transform.position=Vector3.zero;
            transform.rotation=Quaternion.identity;
            //cc.enabled=true;
        }
        else{
            testPhase=true;
            cc.enabled=true;
        }
    }
    void Update()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        if(testPhase)
            {   transform.Rotate(new Vector3(0,50*Time.fixedDeltaTime*Input.GetAxis("Horizontal"),0),Space.World);
                speed=transform.forward*Input.GetAxis("Vertical");  
                speed+=transform.right*Input.GetAxis("Strife");
            
            cc.Move(speed*5.0f*Time.fixedDeltaTime + new Vector3(0,-8.0f*Time.fixedDeltaTime,0));
        } //+new Vector3(0,Mathf.Cos(Time.time)*0.02f*Time.fixedDeltaTime,0)
    }
}
