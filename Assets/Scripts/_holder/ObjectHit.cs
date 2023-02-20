using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collisionData)
    {
        string theTag = collisionData.collider.tag;
        if (theTag != string.Empty)
        {
            Debug.Log("Ow that hurt. Hit a " + theTag);
        } else
        {
            Debug.Log("Hit something unknown");
        }
    }

}
