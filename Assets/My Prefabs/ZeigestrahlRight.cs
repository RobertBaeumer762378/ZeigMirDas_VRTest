using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ZeigestrahlR : MonoBehaviour
{
    public InputActionReference gripRRefference;
    public InputActionReference triggerRRefference;
    public GameObject rightPointer;

    private float gripRValue;
    private float triggerRValue;

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void StartPointing()
    {
        gripRValue = gripRRefference.action.ReadValue<float>();
        triggerRValue = gripRRefference.action.ReadValue<float>();
        if (gripRValue >= 0.5)
        {
            rightPointer.SetActive(true);
        }
    }

    private void StopPointing()
    {
        gripRValue = gripRRefference.action.ReadValue<float>();
        triggerRValue = gripRRefference.action.ReadValue<float>();
        if (gripRValue < 0.5)
        {
            rightPointer.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        StartPointing();
        StopPointing();
    }
}
