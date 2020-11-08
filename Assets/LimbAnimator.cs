using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    Limb limb;
    public LimbAnimator limb2;
    public Transform effector;
    public Transform target;
    public Vector3 dest=Vector3.zero;
    public bool locked=false;
    public float length=0,height,maxDistance; //protected
    int pt=0;
    float[] tempMaxDist=new float[4];
    public bool reset=false; //protected
    public bool main=false;
    protected void Start()
    {
        limb=GetComponent<Limb>();
        effector=limb.effector;
        for(int i=0;i<=limb.bones.GetUpperBound(0);i++)
        {
            length+=limb.bones[i].length;
        }
        float distance=Vector3.Distance(transform.position, effector.position);
        height=transform.position.y-effector.position.y;
        tempMaxDist[0]=length*Mathf.Cos(Mathf.Asin(height/length)) - distance*Mathf.Cos(Mathf.Asin(height/distance)); 
        tempMaxDist[1]=length*Mathf.Cos(Mathf.Asin(distance/length));
        tempMaxDist[2]=length*Mathf.Cos(Mathf.Asin(height/distance));
        tempMaxDist[3]=distance*Mathf.Cos(Mathf.Asin(height/length));//-
        maxDistance=tempMaxDist[1];
        if(float.IsNaN(maxDistance) || maxDistance<=0.001f){
            maxDistance=0.3f;
        }
        main=!limb2.main;
        //target.position+=Vector3.forward*maxDistance/2;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            maxDistance=tempMaxDist[(++pt)%2];
        }
        if(((limb2.locked && Vector3.Distance(effector.position,target.position)>=maxDistance*2 &&   //!locked && || reset
             Vector3.Distance(limb2.effector.position,limb2.target.position)>=maxDistance)) ){
            locked=false;
            dest=target.position;
        }
        moveLimb();
    }

    void moveLimb()
    {
        effector.position=Vector3.MoveTowards(effector.position,dest,Time.fixedDeltaTime*20);
        if(effector.position==dest && !reset){ //(limb2.locked) && 
            locked=true;
        }
    }
}
