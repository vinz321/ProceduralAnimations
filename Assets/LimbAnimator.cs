using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    Limb limb;
    public LimbAnimator limb2;
    Transform effector;
    public Transform target;
    public Vector3 dest=Vector3.zero;
    public bool locked=false;
    protected float length,height,maxDistance;
    protected void Start()
    {
        limb=GetComponent<Limb>();
        effector=limb.effector;
        for(int i=0;i<=limb.bones.GetUpperBound(0);i++)
        {
            length+=limb.bones[i].length;
        }
        height=Vector3.Distance(transform.position, effector.position);
        maxDistance=length*Mathf.Cos(Mathf.Asin(height/length));
        //target.position+=Vector3.forward*maxDistance/2;
        if(!locked)
        {
            //dest=effector.position+Vector3.forward*maxDistance/2;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if(Vector3.Distance(effector.position,target.position)>maxDistance && Vector3.Distance(limb2.effector.position,limb2.target.position)>maxDistance/2)
        {
            dest=target.position;
        }
        if(!locked)
            moveLimb();
    }

    void moveLimb()
    {
        effector.position=Vector3.MoveTowards(effector.position,dest,Time.deltaTime*20);
        if(limb2.locked && Vector3.Distance(effector.position,dest)<0.2f){
            locked=true;
            limb2.locked=false;
        }
    }
}
