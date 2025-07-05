using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionCheckRay : MonoBehaviour
{

    public GameObject ray;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(ray.transform.position, ray.transform.forward, out hit))
        {
            if(hit.collider.tag == "Button")
            {
                Debug.Log("Trigger mit " + hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("Schwanz");
            }
        }
        else
        {
            Debug.Log("Daneben");
        }
    }

    
    }
