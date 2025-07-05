using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject myGameObject;
    public InputAction InputAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ray")
        {
            Debug.Log("Trigger mit " + col.gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Ray")
        {
            Debug.Log("Kollision mit " + col.gameObject.name);
        }
    }

}
