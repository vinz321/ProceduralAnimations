using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbAnimatorController : LimbAnimator
{
    public CreatureController cc;
    Vector3 localTargetPosition;
    new void Start()
    {
        base.Start();
        localTargetPosition=target.position;
    }
    new void FixedUpdate()
    {
        
        base.FixedUpdate();
        Vector3 origin=localTargetPosition+transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance)+new Vector3(0,transform.position.y-effector.position.y,0);// /2
        RaycastHit hit;
        Physics.Raycast(transform.parent.TransformPoint(origin),Vector3.down,out hit,(transform.position.y-effector.position.y)*2,1);
        target.position=hit.point;
        if(cc.speed.sqrMagnitude<0.01f){
            
            if(!reset){
                target.localPosition=localTargetPosition;
                dest=target.position;
            }
            reset=true;
        }
        else{
            
            if(reset && main){
                target.position=hit.point;
                dest=target.position;
                locked=false;
                print("started");
            }

            reset=false;
        }
    }
    public void OnDrawGizmos(){
        
            Gizmos.color=Color.red;
            Gizmos.DrawSphere(transform.parent.TransformPoint(localTargetPosition),0.2f);
            Gizmos.color=Color.green;
            Gizmos.DrawSphere(target.position,0.2f);
            Gizmos.color=Color.blue;
            Gizmos.DrawSphere(dest,0.2f);
        
    }
}
