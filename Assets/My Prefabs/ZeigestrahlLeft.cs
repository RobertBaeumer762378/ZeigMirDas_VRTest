using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ZeigestrahlL : MonoBehaviour
{
    public InputActionReference gripLRefference;
    public InputActionReference triggerLRefference;
    public GameObject leftPointer;

    private float gripLValue;
    private float triggerLValue;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void StartPointing()
    {
        gripLValue = gripLRefference.action.ReadValue<float>();
        triggerLValue = gripLRefference.action.ReadValue<float>();
        if (gripLValue >= 0.5)
        {
            leftPointer.SetActive(true);
        }
    }

    private void StopPointing()
    {
        gripLValue = gripLRefference.action.ReadValue<float>();
        triggerLValue = gripLRefference.action.ReadValue<float>();
        if (gripLValue < 0.5)
        {
            leftPointer.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        StartPointing();
        StopPointing();
    }
}
