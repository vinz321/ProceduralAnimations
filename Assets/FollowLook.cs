using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLook : MonoBehaviour
{
  
    public float length=1;
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 head{
        get{
            return transform.position;
        }
        set{
            transform.position=value;
        }
    }
    public Vector3 tail{
        get{
            return transform.position+transform.forward*length;
        }
        set{
            transform.position=value-transform.forward*length;
        }
    }
    public Vector3 mid{
        get{
            return transform.position+transform.forward*length/2;
        }
        set{
            transform.position=value-transform.forward*length/2;
        }
    }
}
