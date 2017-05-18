using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetIdle()
    {
        animator.SetFloat("speed", 0.0f);
    }

    void SetWalking()
    {
        animator.SetFloat("speed", 0.5f);
    }
}
