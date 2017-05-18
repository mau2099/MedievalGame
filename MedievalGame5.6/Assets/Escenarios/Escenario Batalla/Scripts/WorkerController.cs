using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{

    public Animator animationControl;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


       // if (Input.GetButtonDown("Fire1"))
       // {
       //     animationControl.SetBool("isMinning", true);
       // }
       //else  if (Input.GetButtonDown("Jump"))
       // {
       //     animationControl.SetBool("isMinning", false);
       // }

    }

    void WorkerWalk()
    {
        Debug.Log("isWalking: si "); // + isWalking);
        animationControl.SetFloat("workerSpeed", 1.0f);
        //animationControl.SetFloat("workerSpeed", isWalking ? 0.5f : 0.0f);
    }
}
