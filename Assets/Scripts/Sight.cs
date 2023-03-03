using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance = 3f;
    public float angle = 60f;
    public float distanceDivisorFirst = 3f;
    public bool firstSighting = true;
    public LayerMask objectsLayers;
    public LayerMask obstancesLayers;
    public Collider detectedObject;
    public float distanceFirst = 0f;
    private void Awake()
    {
        if(Mathf.Round(distanceDivisorFirst) > 0f)
        {
            distanceFirst = Mathf.Round((distance/distanceDivisorFirst) * 100.0f) * 0.01f;
        } else
        {
            distanceFirst = 0f;
            firstSighting = false;
        }
     //   Debug.Log("DistanceDiv " + distanceDiv);
    }

    private void Update()
    {
        Collider[] colliders;

        if(firstSighting)
        {
            colliders = Physics.OverlapSphere(transform.position, distanceFirst, (int)objectsLayers);
        }
        else
        {
            colliders = Physics.OverlapSphere(transform.position, distance, (int)objectsLayers);
        }

        detectedObject = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToController = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(transform.forward, directionToController);

            if (angleToCollider < angle)
            {
                if (!Physics.Linecast(transform.position, collider.bounds.center, (int)obstancesLayers))
                {
                    detectedObject = collider;
                    firstSighting = false;
                    Debug.Log("Found " + collider.gameObject.name);
                    break;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, angle);
    }
}
