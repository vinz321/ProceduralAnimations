using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLook : MonoBehaviour
{
    private float _length=1;
    private Limb _limb;
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Limb limb{
        get{
            return _limb;
        }
        set{
            _limb=value;
        }
    }
    public Vector3 constraint{
        get{
            return transform.position-transform.forward*_length/2;
        }
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
            return transform.position+transform.forward*_length;
        }
        set{
            transform.position=value-transform.forward*_length;
        }
    }
    public Vector3 mid{
        get{
            return transform.position+transform.forward*_length/2;
        }
        set{
            transform.position=value-transform.forward*_length/2;
        }
    }
    public float length{
        get {
            return _length;
        }
        set{
            transform.localScale=new Vector3(transform.localScale.x,transform.localScale.y,value);
            _length=value;
        }
    }
}
