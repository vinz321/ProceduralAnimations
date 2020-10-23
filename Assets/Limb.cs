using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    // Start is called before the first frame update
    public FollowLook[] bones;
    
    public Transform effector;
    Transform[] joints;
    float maxDistance;
    void Start()
    {
        transform.up=-(effector.position-transform.position).normalized;
        transform.forward=Vector3.Cross(transform.up,-transform.parent.right);
        joints=new Transform[bones.GetUpperBound(0)+1];
        for(int i=0;i<=bones.GetUpperBound(0);i++)
        {
            joints[i]=Instantiate(new GameObject(),bones[i].head,Quaternion.identity,transform).transform;
        }
        //joints[bones.GetUpperBound(0)]=Instantiate(new GameObject(),effector.position,Quaternion.identity,transform).transform;
        maxDistance=Vector3.Distance(transform.position,effector.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale=new Vector3(1,Vector3.Distance(transform.position, effector.position)/maxDistance,1);
        transform.up=-(effector.position-transform.position).normalized;
        transform.forward=Vector3.Cross(transform.up,-transform.parent.right);
        moveLimb();
    }
    void moveLimb()
    {
        bones[bones.GetUpperBound(0)].transform.forward=(effector.position-joints[bones.GetUpperBound(0)].position).normalized;
        bones[bones.GetUpperBound(0)].tail=effector.position;
        for(int i=bones.GetUpperBound(0)-1;i>=0;i--)
        {
            float zrot=bones[i].transform.rotation.eulerAngles.z;
            bones[i].transform.forward=(bones[i+1].head-joints[i].position).normalized;
            bones[i].tail=bones[i+1].head;
            bones[i].transform.rotation.eulerAngles.Set(bones[i].transform.rotation.eulerAngles.x,bones[i].transform.rotation.eulerAngles.y,zrot);
        }
        bones[0].head=transform.position;
        for(int i=1;i<=bones.GetUpperBound(0);i++)
        {
            bones[i].head=bones[i-1].tail;
        }
    }
   /* void OnDrawGizmos(){
        foreach(Transform j in joints)
        {
            Gizmos.DrawSphere(j.position,0.1f);
        }
    }*/
}
    
