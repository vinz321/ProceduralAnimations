using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject bone;
    
    RaycastHit hit;
    FollowLook currentLimb;
    List<FollowLook> limb1=new List<FollowLook>();
    List<FollowLook> limb2=new List<FollowLook>();
    public GameObject limbAttach;
    FollowLook symmetryLimb;
    bool limbStarted=false;
    bool testPhase=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            testPhase=!testPhase;
        }
        if(!limbStarted && Input.GetMouseButtonDown(0))
        {
               if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,100,1)){
                    currentLimb=Instantiate(bone,hit.point,Quaternion.identity).GetComponent<FollowLook>();
                    symmetryLimb=Instantiate(bone,Vector3.Reflect(hit.point,Vector3.right),Quaternion.identity).GetComponent<FollowLook>();
                    currentLimb.transform.forward=hit.normal;
                    symmetryLimb.transform.forward=Vector3.Reflect(hit.normal,Vector3.right);
                    limb1.Add(currentLimb);
                    limb2.Add(symmetryLimb);
                    limbStarted=true;
                }
        }
        else if(limbStarted && Input.GetMouseButtonDown(0))
        {
            currentLimb=Instantiate(bone,currentLimb.tail,currentLimb.transform.rotation).GetComponent<FollowLook>();
            symmetryLimb=Instantiate(bone,symmetryLimb.tail,symmetryLimb.transform.rotation).GetComponent<FollowLook>();
            limb1.Add(currentLimb);
            limb2.Add(symmetryLimb);
        }
        if(limbStarted && !testPhase){
                currentLimb.transform.Rotate(Vector3.up,Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                currentLimb.transform.Rotate(currentLimb.transform.right,Input.GetAxis("Vertical")*Time.deltaTime*50,Space.World);
                symmetryLimb.transform.Rotate(Vector3.up,-Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                symmetryLimb.transform.Rotate(symmetryLimb.transform.right,Input.GetAxis("Vertical")*Time.deltaTime*50,Space.World);
                currentLimb.transform.localScale-=new Vector3(0,0,Input.GetAxis("MouseScrollWheel")*Time.deltaTime*10);
                symmetryLimb.transform.localScale-=new Vector3(0,0,Input.GetAxis("MouseScrollWheel")*Time.deltaTime*10);  
                currentLimb.length=currentLimb.transform.localScale.z;
                symmetryLimb.length=symmetryLimb.transform.localScale.z;
                }
        if(Input.GetMouseButtonDown(1))
        {

            limbStarted=false;
            Limb limb1Attach=Instantiate(limbAttach,limb1[0].head,Quaternion.identity,transform).GetComponent<Limb>();
            Limb limb2Attach=Instantiate(limbAttach,limb2[0].head,Quaternion.identity,transform).GetComponent<Limb>();

            limb1Attach.bones=limb1.ToArray();
            limb2Attach.bones=limb2.ToArray();

            limb1Attach.effector=Instantiate(new GameObject("Effector1"),limb1[limb1.Count-1].tail,Quaternion.identity).transform;
            limb2Attach.effector=Instantiate(new GameObject("Effector2"),limb2[limb2.Count-1].tail,Quaternion.identity).transform;

            LimbAnimatorController animator1=limb1Attach.GetComponent<LimbAnimatorController>();
            LimbAnimatorController animator2=limb2Attach.GetComponent<LimbAnimatorController>();
            
            animator1.limb2=animator2;
            animator2.limb2=animator1;

            animator1.locked=true;
            animator2.locked=false;

            animator1.target=Instantiate(new GameObject(),limb1[limb1.Count-1].tail,Quaternion.identity,transform).transform;
            animator2.target=Instantiate(new GameObject(),limb2[limb2.Count-1].tail,Quaternion.identity,transform).transform;

            animator1.dest=limb1[limb2.Count-1].tail;
            animator2.dest=limb2[limb2.Count-1].tail;

            animator1.cc=GetComponent<CreatureController>();
            animator2.cc=GetComponent<CreatureController>();

            limb1.Clear();
            limb2.Clear();
        }
    }
}
