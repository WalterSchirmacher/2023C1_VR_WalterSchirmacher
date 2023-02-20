using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance;
    public float angle;
    public LayerMask objectsLayers;
    public LayerMask obstancesLayers;
    public Collider detectedObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, (int)objectsLayers);

        detectedObjects = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToController = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(transform.forward, directionToController);

            if (angleToCollider < angle)
            {
                if(!Physics.Linecast(transform.position, collider.bounds.center, (int)obstancesLayers))
                {
                    detectedObjects = collider;
                    break;
                }
            }
        }
        if(detectedObjects)
        {
            Debug.Log("I see the following:");
            Debug.Log(detectedObjects.name);
        } else
        {
            Debug.Log("Nothing in sight!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(transform.position, distance);
    }
}
