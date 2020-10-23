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
    new void Update()
    {
        base.Update();
        target.localPosition=localTargetPosition+transform.parent.InverseTransformDirection(cc.speed*maxDistance/2);
        
    }
}
