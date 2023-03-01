using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerSphere : MonoBehaviour
{
    public GameObject myParent;
    public GameObject outerSphere;
    public AIFSM aiFSM;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("Inner is ready to go!");
    }

    private void OnDisable()
    {
        Debug.Log("Inner is going to sleep!");
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Inner Hit a " + collider.gameObject.name + " " + collider.gameObject.tag);
        if (collider.gameObject.name == "PlayerBody")
        {
         //   myParent.InnerPlayerFound = true;
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        
        if (collider.gameObject.name == "PlayerBody")
        {
            Debug.Log("Inner Still seeing a " + collider.gameObject.name + " " + collider.gameObject.tag);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "PlayerBody")
        {
            Debug.Log("Inner No longer see a " + collider.gameObject.name + " " + collider.gameObject.tag);
            
        }
    }

}