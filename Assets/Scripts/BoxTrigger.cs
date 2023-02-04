using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    private void onTriggerEnter(Collider other)
    {
        Debug.Log("Was triggered by: " + other.name);

    }

}
