using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject bone;
    
    RaycastHit hit;
    FollowLook currentLimb,symmetryLimb;
    Stack<FollowLook> limb1=new Stack<FollowLook>();
    Stack<FollowLook> limb2=new Stack<FollowLook>();
    public GameObject limbAttach;
    public GameObject armAttach;
    Vector3 axisScale=Vector3.zero;
    FollowLook firstLimb,firstSymLimb;
    public static bool limbStarted=false;

    LimbAnimator last1;
    LimbAnimator last2;
    bool testPhase=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void removePiece(){
        Destroy(limb1.Pop().gameObject);
        Destroy(limb2.Pop().gameObject);
        currentLimb=limb1.Peek();
        symmetryLimb=limb2.Peek();
    }
    public void addPiece(){
        if(limbStarted)
        {
            currentLimb=Instantiate(bone,currentLimb.tail,currentLimb.transform.rotation).GetComponent<FollowLook>();
            symmetryLimb=Instantiate(bone,symmetryLimb.tail,symmetryLimb.transform.rotation).GetComponent<FollowLook>();
            limb1.Push(currentLimb);
            limb2.Push(symmetryLimb);
        }
    }

    public void switchTesting(){
        testPhase=!testPhase;
    }
    void Update()
    {
        if(!limbStarted && Input.GetMouseButtonDown(0) && !testPhase)
        {
               if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,100,1<<8)){
                    currentLimb=Instantiate(bone,hit.point,Quaternion.identity).GetComponent<FollowLook>();
                    symmetryLimb=Instantiate(bone,Vector3.Reflect(hit.point,Vector3.right),Quaternion.identity).GetComponent<FollowLook>();
                    currentLimb.transform.forward=hit.normal;
                    symmetryLimb.transform.forward=Vector3.Reflect(hit.normal,Vector3.right);
                    limb1.Push(currentLimb);
                    limb2.Push(symmetryLimb);
                    firstLimb=currentLimb;
                    firstSymLimb=symmetryLimb;
                    limbStarted=true;
                }
        }
        if(limbStarted && !testPhase){
                if(Input.GetKeyDown(KeyCode.Alpha1)){
                    axisScale=Vector3.right;
                }
                if(Input.GetKeyDown(KeyCode.Alpha2)){
                    axisScale=Vector3.up;
                }
                if(Input.GetKeyDown(KeyCode.Alpha3)){
                    axisScale=Vector3.forward;
                }
                if(Input.GetKeyDown(KeyCode.Alpha4)){
                    axisScale=Vector3.one;
                }
                
                currentLimb.transform.Rotate(Vector3.up,Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                currentLimb.transform.Rotate(currentLimb.transform.right,Input.GetAxis("Vertical")*Time.deltaTime*50,Space.World);
                symmetryLimb.transform.Rotate(Vector3.up,-Input.GetAxis("Horizontal")*Time.deltaTime*50,Space.World);
                symmetryLimb.transform.Rotate(symmetryLimb.transform.right,Input.GetAxis("Vertical")*Time.deltaTime*50,Space.World);
                currentLimb.transform.localScale-=axisScale*Input.GetAxis("MouseScrollWheel")*Time.deltaTime*50;
                symmetryLimb.transform.localScale-=axisScale*Input.GetAxis("MouseScrollWheel")*Time.deltaTime*50; 
                currentLimb.length=currentLimb.transform.localScale.z;
                symmetryLimb.length=symmetryLimb.transform.localScale.z;
                }
        if(Input.GetMouseButtonDown(1))
        {

            limbStarted=false;
            Limb limb1Attach=Instantiate(limbAttach,firstLimb.head,Quaternion.identity,transform).GetComponent<Limb>();
            Limb limb2Attach=Instantiate(limbAttach,firstSymLimb.head,Quaternion.identity,transform).GetComponent<Limb>();

            limb1Attach.bones=reverseStack(limb1.ToArray());
            limb2Attach.bones=reverseStack(limb2.ToArray());

            limb1Attach.effector=Instantiate(new GameObject("Effector1"),limb1.Peek().tail,Quaternion.identity).transform;
            limb2Attach.effector=Instantiate(new GameObject("Effector2"),limb2.Peek().tail,Quaternion.identity).transform;

            LimbAnimatorController animator1=limb1Attach.GetComponent<LimbAnimatorController>();
            LimbAnimatorController animator2=limb2Attach.GetComponent<LimbAnimatorController>();
            
            animator1.limb2=animator2;
            animator2.limb2=animator1;

            animator1.locked=true;
            animator2.locked=false;

            animator1.target=Instantiate(new GameObject(),limb1.Peek().tail,Quaternion.identity,transform).transform;
            animator2.target=Instantiate(new GameObject(),limb2.Peek().tail,Quaternion.identity,transform).transform;

            animator1.dest=limb1.Peek().tail;
            animator2.dest=limb2.Peek().tail;

            animator1.cc=GetComponent<CreatureController>();
            animator2.cc=GetComponent<CreatureController>();
            last1=animator1;
            last2=animator2;

            limb1.Clear();
            limb2.Clear();
        }
        if(Input.GetMouseButtonDown(2))
        {

            limbStarted=false;
            Limb limb1Attach=Instantiate(armAttach,firstLimb.head,Quaternion.identity,transform).GetComponent<Limb>();
            Limb limb2Attach=Instantiate(armAttach,firstSymLimb.head,Quaternion.identity,transform).GetComponent<Limb>();

            limb1Attach.bones=reverseStack(limb1.ToArray());
            limb2Attach.bones=reverseStack(limb2.ToArray());

            limb1Attach.effector=Instantiate(new GameObject("Effector1"),limb1.Peek().tail,Quaternion.identity,transform).transform;
            limb2Attach.effector=Instantiate(new GameObject("Effector2"),limb2.Peek().tail,Quaternion.identity,transform).transform;

            Arm animator1=limb1Attach.GetComponent<Arm>();
            Arm animator2=limb2Attach.GetComponent<Arm>();
            
            animator1.cc=GetComponent<CreatureController>();
            animator2.cc=GetComponent<CreatureController>();

            animator1.side=true;
            animator2.side=false;

            animator1.target=Instantiate(new GameObject(),limb1.Peek().tail,Quaternion.identity,transform).transform;
            animator2.target=Instantiate(new GameObject(),limb2.Peek().tail,Quaternion.identity,transform).transform;

            animator1.animator=last2;
            animator2.animator=last1;

            limb1.Clear();
            limb2.Clear();
        }
    }
    

    private FollowLook[] reverseStack(FollowLook[] arr){
        int up=arr.GetUpperBound(0);
        FollowLook[] newArr=new FollowLook[up+1];
        for(int i=0;i<=up;i++){
            newArr[i]=arr[up-i];
        }
        return newArr;
    }
}
