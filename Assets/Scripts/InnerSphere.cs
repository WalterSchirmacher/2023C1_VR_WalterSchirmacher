using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerSphere : MonoBehaviour
{
    public BearFoe myParent;
    public GameObject outerSphere;

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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Inner Hit a " + collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.name == "PlayerBody")
        {
            myParent.InnerPlayerFound = true;
            myParent.ChasePlayer();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.name == "PlayerBody")
        {
            Debug.Log("Inner Still seeing a " + collision.gameObject.name + " " + collision.gameObject.tag);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "PlayerBody")
        {
            Debug.Log("Inner No longer see a " + collision.gameObject.name + " " + collision.gameObject.tag);
            myParent.InnerPlayerFound = false;
            
        }
    }

}