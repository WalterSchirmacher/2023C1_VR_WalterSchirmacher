using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
