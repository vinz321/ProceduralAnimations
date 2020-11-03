using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    Limb limb;
    public bool side=false;
    Vector3 center;
    float radius;
    Transform effector;
    public CreatureController cc;
    public Transform target;
    public Arm other;
    float time=0.0f;
    float maxDistance;
    public LimbAnimator animator;
    float length;
    bool thrown=false;
    // Start is called before the first frame update
    void Start()
    {
        limb = GetComponent<Limb>();
        effector=limb.effector;
        center=effector.localPosition;
        for(int i=0;i<=limb.bones.GetUpperBound(0);i++)
        {
            length+=limb.bones[i].length;
        }
        float distance=Vector3.Distance(transform.position,effector.position);
        maxDistance=length*Mathf.Cos(Mathf.Asin(distance/length));
        //maxDistance=animator.maxDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if(cc.speed.sqrMagnitude>0.001f){
            time+=Time.fixedDeltaTime;
            if(side)
                effector.localPosition=center+transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance)*timePeriodicFunc(time);
            else
                effector.localPosition=center-transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance)*timePeriodicFunc(time);
        }*/
        if(!animator.locked){
            thrown=true;
        }

        if(thrown){
            float d=Vector3.Distance(effector.localPosition,center);
            effector.localPosition=Vector3.MoveTowards(effector.localPosition,center+transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance),20.0f*Time.fixedDeltaTime*distanceFunc(d));
        }
        
        if(effector.localPosition==center+transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance)){
            thrown=false;
        }
        
        
        if(!thrown){
                effector.localPosition=Vector3.MoveTowards(effector.localPosition,center-transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance),6.0f*Time.fixedDeltaTime);
            
        }

        //effector.localPosition=center-new Vector3(0,0,1)*cc.speed.sqrMagnitude*maxDistance*timePeriodicFunc(time);
        /*if(side)
            target.localPosition=center+target.InverseTransformDirection(cc.speed.normalized)*maxDistance;//f
        else
            target.localPosition=center-target.InverseTransformDirection(cc.speed.normalized)*maxDistance;*/
        //moveArm();
        //transform.Rotate(0,50*Time.deltaTime,0);
    }

    void moveArm(){
        float d=Vector3.Distance(effector.localPosition,transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance));
        effector.localPosition=Vector3.MoveTowards(effector.localPosition,transform.parent.InverseTransformDirection(cc.speed.normalized*maxDistance),distanceFunc(d));
        if(d/maxDistance<0.0001f){
            side=!side;
        }
    }
    float distanceFunc(float d){
        return Mathf.Cos((d-0.1f)*Mathf.PI/(maxDistance*2));
    }
    float timePeriodicFunc(float time){
        return Mathf.Sin(5.0f*time*Mathf.PI/(maxDistance));
    }
    public void OnDrawGizmos(){
        
            Gizmos.color=Color.red;
            Gizmos.DrawSphere(effector.position,0.2f);
        
    }
}
