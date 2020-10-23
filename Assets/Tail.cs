using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public FollowLook[] bones;
    public Transform effector;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        bones[0].head=transform.position;
        bones[0].transform.LookAt(bones[1].head);

        for(int i=1;i<bones.GetUpperBound(0);i++){
            bones[i].head=bones[i-1].tail;
            bones[i].transform.LookAt(bones[i+1].transform);
        }
        bones[bones.GetUpperBound(0)].head=bones[bones.GetUpperBound(0)-1].tail;
        bones[bones.GetUpperBound(0)].transform.LookAt(effector);
        
        effector.position=bones[bones.GetUpperBound(0)].tail; 

    }
}
