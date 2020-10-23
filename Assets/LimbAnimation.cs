using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbAnimation : MonoBehaviour
{
    public CreatureController creature;
    Limb limb;
    public LimbAnimation limbAnimation;
    public bool locked=false; // perno
    public Transform effector;
    public Transform stepTransform;
    Vector3 dest;
    bool walking;
    bool init=false;
    bool moving=false;
    float maxDistance;
    float height,length=0;
    public Vector3 stepOffset;
    void Start()
    {
        limb=GetComponent<Limb>();
        for(int i=0;i<=limb.bones.GetUpperBound(0);i++)
        {
            length+=limb.bones[i].length;
        }
        height=Vector3.Distance(transform.position, effector.position);
        maxDistance=length*Mathf.Cos(Mathf.Asin(height/length));
    }

    // Update is called once per frame
    void Update()
    {
        if(creature.speed.sqrMagnitude<0.01f)
        {
            moving=false;
            dest=stepTransform.position;
        }
        if(!moving && !locked && creature.speed.sqrMagnitude>0.01f)
        {
            dest=stepTransform.position+creature.speed.normalized*maxDistance;
            moving=true;
            limbAnimation.moving=true;
            
            
        }
        if(!locked && Vector3.Distance(stepTransform.position,effector.position)>maxDistance)
        {
            dest=stepTransform.position+creature.speed.normalized*maxDistance;
            /*locked=true;
            limbAnimation.locked=false;*/
        }

        moveEffector();
    }
    void moveEffector()
    {
        if(!locked || !moving)
            effector.position=Vector3.MoveTowards(effector.position,dest,20.5f*Time.deltaTime);
            if(Vector3.Distance(effector.position,dest)<0.1f)
            {
                locked=true;
                limbAnimation.locked=false;
            }
        
    }

}
