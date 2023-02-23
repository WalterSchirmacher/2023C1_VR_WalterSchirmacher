using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightNew : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        string gTag = other.gameObject.tag;
     //   Debug.Log("I see a: " + gTag + ", and it is a: " + other.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        string gTag = other.gameObject.tag;
     //   Debug.Log("I still see a: " + gTag + ", and it is a: " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        string gTag = other.gameObject.tag;
    //    Debug.Log("I no longer see a: " + gTag + ", and it is a: " + other.gameObject.name);
    }
}
