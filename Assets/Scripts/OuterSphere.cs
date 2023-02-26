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

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Outer Hit a " + collider.gameObject.name + " " + collider.gameObject.tag);
        if (collider.gameObject.name == "PlayerBody")
        {
            myParent.OuterPlayerFound = true;
            innerSphere.SetActive(true);
            myParent.PlayerFound();
            if(myParent.myStatus == GameMaster.Disposition.ExtremeHatred)
            {
                if (!myParent.isChasing)
                {
                    myParent.ChasePlayer();
                }
            }

        }
    }
    private void OnTriggerStay(Collider collider)
    {
        
        if (collider.gameObject.name == "PlayerBody")
        {
            //  Debug.Log("Outer Still seeing a " + collider.gameObject.name + " " + collider.gameObject.tag);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "PlayerBody")
        {
            Debug.Log("Outer No longer see a " + collider.gameObject.name + " " + collider.gameObject.tag);
            myParent.OuterPlayerFound = false;
            innerSphere.SetActive(false);
            myParent.PlayerLost();
        }
    }
}