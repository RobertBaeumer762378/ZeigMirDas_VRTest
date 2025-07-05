using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class animatorStateController : MonoBehaviour
{
    private float timer;
    public float interval;
    public GameObject spitze;
    private Animator animator;
    private InputAction move;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
        bool timePassed = animator.GetBool("TimeHasPassed");
        timer += Time.deltaTime;
        if (timePassed && timer < interval)
        {
            animator.SetBool("TimeHasPassed", false);
        }
        if (timer >= interval)
        {
            timer -= interval;
            animator.SetBool("TimeHasPassed", true);
        }*/
        

        /* if (Input.GetKeyDown(KeyCode.T))
         {
             animator.SetBool("TIsPressed", true);
         }
         else
         {
             animator.SetBool("TIsPressed", false);
         }
        */
    }
}
