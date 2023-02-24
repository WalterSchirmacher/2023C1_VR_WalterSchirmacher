using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterSphere : MonoBehaviour
{
    public BearFoe myParent;
    public GameObject innerSphere;

    private void Start()
    {
        Debug.Log("Outer Starting up.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Outer Hit a " + collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.name == "PlayerBody")
        {
            myParent.OuterPlayerFound = true;
            innerSphere.SetActive(true);
            myParent.PlayerFound();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.name == "PlayerBody")
        {
            Debug.Log("Outer Still seeing a " + collision.gameObject.name + " " + collision.gameObject.tag);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "PlayerBody")
        {
            Debug.Log("Outer No longer see a " + collision.gameObject.name + " " + collision.gameObject.tag);
            myParent.OuterPlayerFound = false;
            innerSphere.SetActive(false);
        }
    }
}